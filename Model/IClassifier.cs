using System.Collections.Generic;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Model
{
  public interface IClassifier
  {
    void CreateClassificationFile(int feedID, eClassificationType actionType, IEnumerable<BaseAuction> auctions);
  }
}
