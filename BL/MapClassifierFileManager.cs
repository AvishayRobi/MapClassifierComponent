using System.Collections.Generic;
using System.Linq;
using MapClassifierComponent.Model;
using OFInfrastructure.Auctions.Model;
using OFInfrastructure.Files;
using WallaShops.Utils;

namespace MapClassifierComponent.BL
{
  public class MapClassifierFileManager : IClassifier
  {
    #region Data Members
    private static readonly string mainLocation = WSGeneralUtils.GetAppSettings("MainLocation");
    #endregion

    #region Public Methods
    public IDictionary<string, BaseAuction> GetLastProducts(int feedID)
    {
      string fullPath = $"{mainLocation}\\{feedID}\\{WSGeneralUtils.GetAppSettings("PreviousState")}\\FileName.json";
      string fileContent = FileUtils.ReadFile(fullPath);

      bool isFileExist = !string.IsNullOrEmpty(fileContent);

      return isFileExist
        ? convertFileContentToDictionary(fileContent)
        : getEmptyDictionary();
    }

    public void CreateClassificationFile(int feedID, eClassificationType actionType, IEnumerable<BaseAuction> auctions)
    {
      string fullPath = $"{mainLocation}\\{feedID}\\{WSGeneralUtils.GetAppSettings("PendingValidation")}\\{actionType.ToString()}\\FileName.json";
      string data = WSJsonConverter.SerializeObject(auctions);

      FileUtils.WriteToFile(fullPath, data);
    }

    public void UpdateLastProductsFile(int feedID, IEnumerable<BaseAuction> auctions)
    {
      string fullPath = $"{mainLocation}\\{feedID}\\{WSGeneralUtils.GetAppSettings("PreviousState")}\\FileName.json";
      string data = WSJsonConverter.SerializeObject(auctions);

      FileUtils.WriteToFile(fullPath, data);
    }
    #endregion

    #region Private Methods
    private IDictionary<string, BaseAuction> convertFileContentToDictionary(string fileContent)
      =>
      fileContent
      .ConvertJsonToEnumerable<BaseAuction>()
      .ToDictionary(a => a.Model, a => a);

    private IDictionary<string, BaseAuction> getEmptyDictionary()
      =>
      new Dictionary<string, BaseAuction>();
    #endregion
  }
}
