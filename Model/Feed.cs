using System.Collections.Generic;
using MapClassifierComponent.InputTypes.Model;
using MapClassifierComponent.Entities;
using MapClassifierComponent.Mapping.Model;

namespace MapClassifierComponent.Model
{
  public class Feed : IExtractableFeed, IMappableFeed
  {
    public IEnumerable<string> Emails { get; set; }

    public int FeedID { get; set; }

    public string FeedName { get; set; }

    public eInputType InputType { get; set; }

    public FeedMap Map { get; set; }

    public eMappingType MappingType { get; set; }

    public int ShopID { get; set; }

    public string Url { get; set; }
  }
}
