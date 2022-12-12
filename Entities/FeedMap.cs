using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MapClassifierComponent.Entities.MapItems;
using MapClassifierComponent.Model;
using WallaShops.Utils;

namespace MapClassifierComponent.Entities
{
  public class FeedMap
  {
    #region Properties
    public IEnumerable<FeedMapItemBrand> Brands => this.brands;
    public IEnumerable<FeedMapItemCategory> Categories => this.categories;
    public string ServiceMap { get; set; }
    #endregion

    #region Data Members
    private IEnumerable<FeedMapItemBrand> brands { get; set; }
    private IEnumerable<FeedMapItemCategory> categories { get; set; }
    #endregion

    #region Ctor
    public FeedMap(SqlDataReader reader)
    {
      IDictionary<int, BaseFeedMapItem> itemTypes = getItemTypes();
      ICollection<BaseFeedMapItem> items = new List<BaseFeedMapItem>();

      do
      {
        while (reader.Read())
        {
          int itemType = WSStringUtils.ToInt(reader["map_item_type"]);
          items.Add(itemTypes[itemType].GetItemByReader(reader));
        }
      } while (reader.NextResult());

      fillMapItems(items);
    }
    #endregion

    #region Data Members
    private void fillMapItems(IEnumerable<BaseFeedMapItem> items)
    {
      this.categories = items
        .Where(isItemTypeCategory)
        .Cast<FeedMapItemCategory>();

      this.brands = items
        .Where(isItemTypeBrand)
        .Cast<FeedMapItemBrand>();
    }

    private IDictionary<int, BaseFeedMapItem> getItemTypes()
      =>
      new Dictionary<int, BaseFeedMapItem>()
      {
        { (int)eFeedMapItem.Category, new FeedMapItemCategory() },
        { (int)eFeedMapItem.Brand, new FeedMapItemBrand() }
      };

    private bool isItemTypeCategory(BaseFeedMapItem item)
      =>
      item.MapItemType == eFeedMapItem.Category;

    private bool isItemTypeBrand(BaseFeedMapItem item)
      =>
      item.MapItemType == eFeedMapItem.Brand;
    #endregion
  }
}
