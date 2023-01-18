using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapClassifierComponent.BL;
using MapClassifierComponent.Classification;
using MapClassifierComponent.Model;
using MapClassifierComponent.InputTypes;
using MapClassifierComponent.InputTypes.Model;
using MapClassifierComponent.Mapping;
using MapClassifierComponent.Mapping.Model;
using OFInfrastructure.Auctions.Model;
using OFInfrastructure.Logs;
using OFInfrastructure.Logs.Model;
using WallaShops.Utils;

namespace MapClassifierComponent
{
  public class MapClassifierHandler
  {
    #region Data Members
    private IDalManager dalManager { get; }
    private IFileManager fileManager { get; }
    private LogManager logManager { get; }
    #endregion

    #region Ctor
    public MapClassifierHandler()
    {
      this.dalManager = DataAccessLayer.GetDalManagerObj();
      this.fileManager = DataAccessLayer.GetFileManagerObj();
      this.logManager = new LogManager();
    }
    #endregion

    #region Public Methods
    public void Exec()
    {
      IEnumerable<Feed> feeds = getExternalFeeds();
      processFeeds(feeds);
    }
    #endregion

    #region Private Methods
    private void processFeeds(IEnumerable<Feed> feeds)
    {
      foreach (Feed feed in feeds)
      {
        try
        {
          lockFeed(feed.FeedID);
          processSingleFeed(feed);
        }
        catch (Exception ex)
        {
          handleException(feed.FeedID, ex);
        }

        unlockFeed(feed.FeedID);
      }
    }

    private void processSingleFeed(Feed feed)
    {
      ClassificationInfo classificationInfo = getClassificationInfo(feed);

      bool isStreamEmpty = !classificationInfo.CurrentFeedAuctions.Any();
      if (isStreamEmpty)
      {
        handleEmptyStream(feed.FeedID);
      }
      else
      {
        classifyAuctions(classificationInfo);
        updateLastAuctionsFile(classificationInfo.CurrentFeedAuctions, feed.FeedID);
      }
    }

    private ClassificationInfo getClassificationInfo(Feed feed)
    {
      IDictionary<string, BaseAuction> previousFeedAuctions = getPreviousFeedAuctions(feed.FeedID);
      IDictionary<string, BaseAuction> currentFeedAuctions = getFeedAuctions(feed);

      return new ClassificationInfo()
      {
        PreviousFeedAuctions = previousFeedAuctions,
        CurrentFeedAuctions = currentFeedAuctions,
        FeedID = feed.FeedID
      };
    }

    private IDictionary<string, BaseAuction> getFeedAuctions(Feed feed)
    {
      Stream feedStream = getFeedStream(feed);
      bool isStreamEmpty = feedStream == null || (feedStream.CanSeek && feedStream.Length == 0);

      return isStreamEmpty
        ? getEmptyCollection()
        : getAuctionsFromStream(feed, feedStream);
    }

    private void updateLastAuctionsFile(IDictionary<string, BaseAuction> currentAuctions, int feedID)
    {
      IEnumerable<BaseAuction> auctions = currentAuctions
        .Select(a => a.Value);

      this.fileManager
        .UpdateLastProductsFile(feedID, auctions);
    }

    private void handleException(int feedID, Exception ex)
    {
      ILoggable log = new FeedLog()
      {
        ProcessName = WSGeneralUtils.GetAppSettings("ProcessName"),
        IndividualIdentifier = feedID.ToString(),
        Exception = ex
      };

      writeLog(log);
    }

    private void handleEmptyStream(int feedID)
    {
      ILoggable log = new FeedLog()
      {
        ProcessName = WSGeneralUtils.GetAppSettings("ProcessName"),
        Exception = new Exception("Empty Stream"),
        IndividualIdentifier = feedID.ToString()
      };

      writeLog(log);
    }

    private Stream getFeedStream(IExtractableFeed feed)
      =>
      new ExtractorManager()
      .SetFeed(feed)
      .GetFeedStream();

    private IDictionary<string, BaseAuction> getAuctionsFromStream(IMappableFeed feed, Stream feedStream)
      =>
      new MapManager()
      .SetFeedStream(feedStream)
      .SetFeed(feed)
      .GetAuctions()
      .ToDictionary(a => a.Model, a => a);

    private void classifyAuctions(ClassificationInfo classificationInfo)
      =>
      new ClassificationManager()
      .SetClassificationInfo(classificationInfo)
      .ClassifyAuctions(classifier: this.fileManager);

    private IDictionary<string, BaseAuction> getPreviousFeedAuctions(int feedID)
      =>
      this.fileManager
      .GetLastProducts(feedID);

    private IEnumerable<Feed> getExternalFeeds()
      =>
      this.dalManager
      .GetExternalFeeds();

    private void lockFeed(int feedID)
      =>
      this.dalManager
      .LockFeed(feedID);

    private void unlockFeed(int feedID)
      =>
      this.dalManager
      .UnlockFeed(feedID);

    private void writeLog(ILoggable log)
      =>
      this.logManager
      .WriteInnerLog(log);

    private IDictionary<string, BaseAuction> getEmptyCollection()
      =>
      Enumerable.Empty<BaseAuction>()
      .ToDictionary(a => a.Model);
    #endregion
  }
}
