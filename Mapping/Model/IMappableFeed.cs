using MapClassifierComponent.Entities;
using MapClassifierComponent.Model;

namespace MapClassifierComponent.Mapping.Model
{
  public interface IMappableFeed
  {
    string FeedName { get; set; }

    FeedMap Map { get; set; }

    eMappingType MappingType { get; set; }

    int ShopID { get; set; }
  }
}
