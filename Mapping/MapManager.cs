using System.Collections.Generic;
using System.IO;
using MapClassifierComponent.Mapping.Mappers;
using MapClassifierComponent.Mapping.Model;
using MapClassifierComponent.Model;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Mapping
{
  public class MapManager
  {
    #region Data Members
    private IMappableFeed feed { get; set; }
    private Stream feedStream { get; set; }
    #endregion

    #region Ctor
    public MapManager()
    {
      this.feedStream = Stream.Null;
    }
    #endregion

    #region Public Methods
    public MapManager SetFeed(IMappableFeed feed)
    {
      this.feed = feed;

      return this;
    }

    public MapManager SetFeedStream(Stream stream)
    {
      this.feedStream = stream;

      return this;
    }

    public IEnumerable<BaseAuction> GetAuctions()
    {
      IFeedMapper mapper = getMapper();

      return mapper
        .SetStream(this.feedStream)
        .SetFeed(this.feed)
        .Transform();
    }
    #endregion

    #region Private Methods
    private IFeedMapper getMapper()
    {
      IFeedMapper mapper = null;

      switch (this.feed.MappingType)
      {
        case eMappingType.Admonis:
          mapper = new AdmonisMapper();
          break;
        case eMappingType.Konimbo:
          mapper = new KonimboMapper();
          break;
      }

      return mapper;
    }
    #endregion
  }
}
