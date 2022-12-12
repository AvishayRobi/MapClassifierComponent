using System.Collections.Generic;
using System.IO;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Mapping.Model
{
  public interface IFeedMapper
  {
    IFeedMapper SetFeed(IMappableFeed feed);

    IFeedMapper SetStream(Stream stream);

    IEnumerable<BaseAuction> Transform();
  }
}
