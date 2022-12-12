using System.Data.SqlClient;
using MapClassifierComponent.Model;
using WallaShops.Utils;

namespace MapClassifierComponent.Entities.MapItems
{
  public class FeedMapItemCategory : BaseFeedMapItem
  {
    #region Properties
    public string FeedCategoryName => this.feedCategoryName;
    public int SFTarget => this.sfTarget;
    public int BegoTarget => this.begoTarget;
    #endregion

    #region Data Members
    private string feedCategoryName { get; }
    private int sfTarget { get; }
    private int begoTarget { get; }
    #endregion

    #region Ctor
    public FeedMapItemCategory()
    {
    }
      
    public FeedMapItemCategory(SqlDataReader reader)
    {
      base.MapItemType = eFeedMapItem.Category;
      this.feedCategoryName = WSStringUtils.ToString(reader["feed_category_name"]);
      this.sfTarget = WSStringUtils.ToInt(reader["sf_target"]);
      this.begoTarget = WSStringUtils.ToInt(reader["bego_target"]);
    }
    #endregion

    #region Public Methods  
    public override BaseFeedMapItem GetItemByReader(SqlDataReader reader)
      =>
      new FeedMapItemCategory(reader);
    #endregion
  }
}
