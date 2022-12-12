using System.Data;
using System.Data.SqlClient;
using WallaShops.Data;
using WallaShops.Objects;

namespace MapClassifierComponent.DAL
{
  public class MapClassifierDal : WSSqlHelper
  {
    #region Ctor
    public MapClassifierDal() : base(WSPlatforms.WallaShops)
    {
    }
    #endregion

    #region Public Methods
    public void LockFeed(int feedID)
    {
    }

    public void UnlockFeed(int feedID)
    {
    }

    public SqlDataReader GetFeedMapReader(int feedID)
    {
      WSSqlParameters spParams = new WSSqlParameters();
      spParams.AddInputParameter("@feed_id", feedID);

      return base.ExecuteReader("", ref spParams);
    }

    public DataTable GetExternalFeeds()
      =>
      base.GetDataTable("");
    #endregion
  }
}
