using System.Collections.Generic;
using MapClassifierComponent.Model;

namespace MapClassifierComponent.Classification.Model
{
  public abstract class BaseClassifiedAuctionsCollection<T> : List<T>, IClassifiableCollection<T>
  {
    #region Data Members
    protected int feedID { get; set; }
    #endregion

    #region Ctor
    protected BaseClassifiedAuctionsCollection()
    {      
    }
    #endregion

    #region Public Methods
    public abstract void Classify(IClassifier classifier);

    public IClassifiableCollection<T> SetIdentifier(int identifier)
    {
      this.feedID = identifier;

      return this;
    }
    #endregion
  }
}
