using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MapClassifierComponent.DAL;
using MapClassifierComponent.Entities;
using MapClassifierComponent.Model;
using WallaShops.Utils;

namespace MapClassifierComponent.BL
{
  public class MapClassifierDalManager
  {
    #region Data Members
    private MapClassifierDal dal { get; }
    #endregion

    #region Ctor
    public MapClassifierDalManager()
    {
      this.dal = new MapClassifierDal();
    }
    #endregion

    #region Public Methods
    public IEnumerable<Feed> GetExternalFeeds()
    {
      DataTable dt = this.dal.GetExternalFeeds();

      return from DataRow dr
             in dt.Rows
             select new Feed()
             {
               MappingType = extractMappingType(dr["mapping_type"]),
               FeedName = WSStringUtils.ToString(dr["feed_name"]),
               InputType = extractInputType(dr["input_type"]),
               FeedID = WSStringUtils.ToInt(dr["feed_id"]),
               ShopID = WSStringUtils.ToInt(dr["shop_id"]),
               Url = WSStringUtils.ToString(dr["url"]),
               Emails = extractEmails(dr["emails"]),
               Map = getFeedMap(dr)
             };
    }

    public void LockFeed(int feedID)
      =>
      this.dal
      .LockFeed(feedID);

    public void UnlockFeed(int feedID)
      =>
      this.dal
      .UnlockFeed(feedID);
    #endregion

    #region Private Methods
    private FeedMap getFeedMap(DataRow dr)
    {
      int feedID = WSStringUtils.ToInt(dr["feed_id"]);
      SqlDataReader reader = this.dal.GetFeedMapReader(feedID);
      FeedMap map = null;

      using (reader)
      {
        map = new FeedMap(reader);
      }

      map.ServiceMap = WSStringUtils.ToString(dr["service_map"]);

      return map;
    }

    private IEnumerable<string> extractEmails(object dataRow)
      =>
      WSStringUtils.ToString(dataRow)
      .Split(';');

    private eInputType extractInputType(object dataRow)
      =>
      (eInputType)WSStringUtils.ToInt(dataRow);

    private eMappingType extractMappingType(object dataRow)
      =>
      (eMappingType)WSStringUtils.ToInt(dataRow);
    #endregion
  }
}
