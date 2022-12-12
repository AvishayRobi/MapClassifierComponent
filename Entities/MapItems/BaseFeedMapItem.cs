using System.Data.SqlClient;
using MapClassifierComponent.Model;

namespace MapClassifierComponent.Entities.MapItems
{
  public abstract class BaseFeedMapItem
  {
    #region Properties
    public eFeedMapItem MapItemType
    {
      get => this.mapItemType;
      set => this.mapItemType = value;
    }
    #endregion

    #region Data Members
    private eFeedMapItem mapItemType { get; set; }
    #endregion

    #region Ctors
    protected BaseFeedMapItem()
    {
    }
    #endregion

    #region Public Methods
    public abstract BaseFeedMapItem GetItemByReader(SqlDataReader reader);
    #endregion
  }
}
