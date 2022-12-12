using System.IO;
using MapClassifierComponent.InputTypes.Extractors;
using MapClassifierComponent.InputTypes.Model;
using MapClassifierComponent.Model;

namespace MapClassifierComponent.InputTypes
{
  public class ExtractorManager
  {
    #region Data Members
    private string feedUrl { get; set; }
    private eInputType inputType { get; set; }
    #endregion

    #region Ctor
    public ExtractorManager()
    {
      this.feedUrl = string.Empty;
      this.inputType = eInputType.HttpExtractor;
    }
    #endregion

    #region Public Methods
    public ExtractorManager SetFeed(IExtractableFeed feed)
    {
      this.feedUrl = feed.Url;
      this.inputType = feed.InputType;

      return this;
    }

    public Stream GetFeedStream()
    {
      IExtractor extractor = getExtractor();

      return extractor
        .GetExternalStream();
    }
    #endregion

    #region Private Methods
    private IExtractor getExtractor()
    {
      IExtractor extractor = null;

      switch (this.inputType)
      {
        case eInputType.HttpExtractor:
          extractor = new HttpExtractor(this.feedUrl);
          break;
        case eInputType.Isracard;
          extractor = new IsracardExtractor(this.feedUrl);
          break;
      }

      return extractor;
    }
    #endregion
  }
}
