using System.Collections.Generic;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Model
{
  public class ShallowAuctionComparer : IEqualityComparer<BaseAuction>
  {
    public bool Equals(BaseAuction auc1, BaseAuction auc2)
    {
      if (object.ReferenceEquals(auc1, auc2))
      {
        return true;
      }

      if (object.ReferenceEquals(auc1, null) || object.ReferenceEquals(auc2, null))
      {
        return false;
      }

      return auc1.PageDetails.ShortDescription == auc2.PageDetails.ShortDescription
        && auc1.AttributeCombinations.Count == auc2.AttributeCombinations.Count
        && auc1.CategoryTree.FeedCategory == auc2.CategoryTree.FeedCategory
        && auc1.PageDetails.Description == auc2.PageDetails.Description
        && auc1.Prices.OriginalPrice == auc2.Prices.OriginalPrice
        && auc1.Prices.StartPrice == auc2.Prices.StartPrice
        && auc1.Prices.CostPrice == auc2.Prices.CostPrice
        && auc1.PageDetails.Name == auc2.PageDetails.Name
        && auc1.StockAmount == auc2.StockAmount
        && auc1.Brand == auc2.Brand;
    }

    public int GetHashCode(BaseAuction auction)
      =>
      auction != null
      ? auction.Model.GetHashCode()
      : 0;
  }
}
