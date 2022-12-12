using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MapClassifierComponent.Classification.Model;
using MapClassifierComponent.Extensions;
using MapClassifierComponent.Model;
using OFInfrastructure.Auctions.Model;
using WallaShops.Utils;

namespace MapClassifierComponent.Classification
{
  public class ClassificationManager
  {
    #region Data Members
    private IDictionary<string, BaseAuction> currentAuctions { get; set; }
    private int feedID { get; set; }
    private IDictionary<string, BaseAuction> previousAuctions { get; set; }
    #endregion

    #region Ctor
    public ClassificationManager()
    {
      this.currentAuctions = new Dictionary<string, BaseAuction>();
      this.previousAuctions = new Dictionary<string, BaseAuction>();
    }
    #endregion

    #region Public Methods
    public ClassificationManager SetClassificationInfo(ClassificationInfo classificationInfo)
    {
      this.previousAuctions = classificationInfo.PreviousFeedAuctions;
      this.currentAuctions = classificationInfo.CurrentFeedAuctions;
      this.feedID = classificationInfo.FeedID;

      return this;
    }

    public void ClassifyAuctions(IClassifier classifier)
    {
      IEnumerable<IClassifiableCollection<BaseAuction>> classifiedAuctionBulks = getClassifiedAuctions();

      classifiedAuctionBulks
        .Where(isAuctionBulkNotEmpty)
        .ApplyEach(ab =>
        {
          createClassicifationFiles(ab, classifier);
        });
    }
    #endregion

    #region Private Methods
    private IEnumerable<IClassifiableCollection<BaseAuction>> getClassifiedAuctions()
    {
      IClassifiableCollection<BaseAuction> newAuctions = getNewAuctions();
      IClassifiableCollection<BaseAuction> auctionsToDelete = getAuctionsToDelete();
      IClassifiableCollection<BaseAuction> auctionsToReplace = getAuctionsToReplace();

      return new Collection<IClassifiableCollection<BaseAuction>>()
      {
        newAuctions,
        auctionsToDelete,
        auctionsToReplace
      };
    }

    private IClassifiableCollection<BaseAuction> getAuctionsToReplace()
    {
      IEnumerable<BaseAuction> currentFeedAuctions = getCurrentAuctions();
      IEqualityComparer<BaseAuction> comparer = getAuctionComparer();

      return this.previousAuctions
        .Select(extractAuction)
        .Except(currentFeedAuctions, comparer)
        .ToClassifiableCollection<ClassifiedAuctionsCollectionReplace<BaseAuction>>();
    }

    private IClassifiableCollection<BaseAuction> getNewAuctions()
      =>
      this.currentAuctions
      .Where(isNotInPreviousAuctions)
      .Select(extractAuction)
      .ToClassifiableCollection<ClassifiedAuctionsCollectionInsert<BaseAuction>>();

    private IClassifiableCollection<BaseAuction> getAuctionsToDelete()
      =>
      this.previousAuctions
      .Where(isNotInCurrentAuctions)
      .Select(extractAuction)
      .ToClassifiableCollection<ClassifiedAuctionsCollectionDelete<BaseAuction>>();

    private void createClassicifationFiles(IClassifiableCollection<BaseAuction> auctionsBulk, IClassifier classifier)
      =>
      auctionsBulk
      .SetIdentifier(this.feedID)
      .Classify(classifier: classifier);

    private IEqualityComparer<BaseAuction> getAuctionComparer()
      =>
      new ShallowAuctionComparer();

    private IEnumerable<BaseAuction> getCurrentAuctions()
      =>
      this.currentAuctions
      .Select(a => a.Value);

    private BaseAuction extractAuction(KeyValuePair<string, BaseAuction> auctionValuePair)
      =>
      auctionValuePair.Value;

    private bool isNotInPreviousAuctions(KeyValuePair<string, BaseAuction> auctionValuePair)
      =>
      !this.previousAuctions
      .ContainsKey(auctionValuePair.Value.Model);

    private bool isNotInCurrentAuctions(KeyValuePair<string, BaseAuction> auctionValuePair)
      =>
      !this.currentAuctions
      .ContainsKey(auctionValuePair.Value.Model);

    private bool isAuctionBulkNotEmpty(IClassifiableCollection<BaseAuction> auctionBulk)
      =>
      auctionBulk.Any();
    #endregion
  }
}
