using System.Collections.Generic;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Model
{
  public class ClassificationInfo
  {
    public IDictionary<string, BaseAuction> CurrentFeedAuctions { get; set; }

    public int FeedID { get; set; }

    public IDictionary<string, BaseAuction> PreviousFeedAuctions { get; set; }
  }
}
