using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using OFInfrastructure.Auctions.Model;
using MapClassifierComponent.Model;
using WallaShops.Utils;
using WallaShops.Objects;

namespace MapClassifierComponent.Entities
{
  public class FeedAuctionManager
  {
    #region Properties
    public BaseAuction Auction => this.auction;
    #endregion

    #region Data Members
    private BaseAuction auction { get; }
    private FeedMap map { get; }
    #endregion

    #region Ctor
    public FeedAuctionManager(FeedMap map, string auctionSource)
    {
      this.auction = new DryAuction();
      this.auction.AuctionSource = auctionSource;

      this.map = map;
    }
    #endregion

    #region Public Methods
    public void SetServiceCategory(string feedCategory)
    {      
    }

    public void SetSFCategory(string feedCategory)
    {
      var categoryTargets = this.map
        .Categories
        .Where(c => c.FeedCategoryName.ToComparable() == feedCategory.ToComparable())
        .Select(c => new
        {
          SFTarget = c.SFTarget,
          BegoTarget = c.BegoTarget
        })
        .FirstOrDefault();

      this.auction.CategoryTree.Storefront.ID = categoryTargets?.SFTarget ?? WSGeneralUtils.GetAppSettingsInt("DefaultSFCategory");
      this.auction.CategoryTree.Bego.ID = categoryTargets?.BegoTarget ?? this.auction.CategoryTree.Storefront.ID;
      this.auction.CategoryTree.FeedCategory = feedCategory;
    }

    public void SetBrand(string feedBrand)
    {
      this.auction.Brand = this.map
        .Brands
        .FirstOrDefault(b => b.FeedBrand.ToComparable() == feedBrand.ToComparable())
        ?.Target
        ?? feedBrand;
    }

    public void SetStockAmount(string stockAmount = "")
    {
      bool isCustomStock = int.TryParse(stockAmount, out int quantity);

      this.auction.StockAmount =
        isCustomStock
        ? quantity
        : WSGeneralUtils.GetAppSettingsInt("DefaultCustomStockAmount");
    }

    public void SetShipping(WSShipmentMethodsType method, int cost, int shippingTime)
    {
      this.auction.Shipment.ShippingTime = shippingTime;

      this.auction.Shipment.ShippingMethods = new List<WSAPIManagerAuctionShipment>()
      {
        new WSAPIManagerAuctionShipment()
        {
          Method = WSShipmentMethodsType.SelfShipping,
          Price = 0
        },
        new WSAPIManagerAuctionShipment()
        {
          Method = method,
          Price = cost
        }
      };
    }

    public void SetMarketplace()
    {
      this.auction.Prices.CostPrice = this.auction.Prices.StartPrice;
      this.auction.IsMarketPlace = true;
    }
    #endregion
  }
}
