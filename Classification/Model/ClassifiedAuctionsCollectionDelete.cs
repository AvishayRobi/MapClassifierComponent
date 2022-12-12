using System.Collections.Generic;
using MapClassifierComponent.Model;
using OFInfrastructure.Auctions.Model;

namespace MapClassifierComponent.Classification.Model
{
  public class ClassifiedAuctionsCollectionDelete<T> : BaseClassifiedAuctionsCollection<T>
  {
    public override void Classify(IClassifier classifier)
      =>
      classifier
      .CreateClassificationFile(base.feedID, eClassificationType.Delete, this as IEnumerable<BaseAuction>);
  }
}
