using MapClassifierComponent.Model;

namespace MapClassifierComponent.InputTypes.Model
{
  public interface IExtractableFeed
  {
    eInputType InputType { get; set; }

    string Url { get; set; }
  }
}
