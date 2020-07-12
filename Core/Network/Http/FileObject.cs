using System;
using System.IO;

namespace Core.Network.Http
{
  public class FileObject
  {
    public byte[] Data { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    public Stream Stream { get; set; }
    public object Extras { get; set; }
  }
}
