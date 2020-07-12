using System;
namespace Core.Tools
{
  public interface IDocumentHelper
  {
    void OpenLocalDocument(string path, Action completed = null);
  }
}
