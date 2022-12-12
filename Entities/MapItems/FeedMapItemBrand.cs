using System.Data.SqlClient;
using MapClassifierComponent.Model;
using WallaShops.Utils;

namespace MapClassifierComponent.Entities.MapItems
{
  public class FeedMapItemBrand : BaseFeedMapItem
  {
    #region Properties
    public string FeedBrand => this.feedBrand;
    public string Target => this.target;
    #endregion

    #region Data Members
    private string feedBrand { get; }
    private string target { get; }
    #endregion

    #region Ctor
    public FeedMapItemBrand()
    {
    }

    public FeedMapItemBrand(SqlDataReader reader)
    {
      base.MapItemType = eFeedMapItem.Brand;
      this.feedBrand = WSStringUtils.ToString(reader["feed_brand"]);
      this.target = WSStringUtils.ToString(reader["target"]);
    }
    #endregion

    #region Public Methods  
    public override BaseFeedMapItem GetItemByReader(SqlDataReader reader)
      =>
      new FeedMapItemBrand(reader);
    #endregion
  }
}
