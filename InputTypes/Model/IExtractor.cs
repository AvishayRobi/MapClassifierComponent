using System.IO;

namespace MapClassifierComponent.InputTypes.Model
{
  public interface IExtractor
  {
    Stream GetExternalStream();
  }
}
