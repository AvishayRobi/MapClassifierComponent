using System.Collections.Generic;

namespace MapClassifierComponent.Model
{
  public interface IClassifiableCollection<T> : IEnumerable<T>
  {
    void Add(T item);

    void Classify(IClassifier classifier);

    IClassifiableCollection<T> SetIdentifier(int identifier);
  }
}
