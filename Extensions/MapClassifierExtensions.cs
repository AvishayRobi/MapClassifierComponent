using System.Collections.Generic;
using MapClassifierComponent.Classification.Model;
using MapClassifierComponent.Model;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Extensions
{
  public static class MapClassifierExtensions
  {
    public static IClassifiableCollection<BaseAuction> ToClassifiableCollection<T>(this IEnumerable<BaseAuction> source) 
      where T : BaseClassifiedAuctionsCollection<BaseAuction>, new()
    {
      IClassifiableCollection<BaseAuction> collection = new T();

      foreach (var item in source)
      {
        collection.Add(item);
      }

      return collection;
    }
  }
}
