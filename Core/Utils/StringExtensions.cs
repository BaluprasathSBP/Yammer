using System;
using System.Collections.Generic;
using System.Web;
using HtmlAgilityPack;

namespace Core.Utils
{
  public static class StringExtensions
  {
    public static string StripHtml(this string input)
    {
      if (string.IsNullOrEmpty(input))
      {
        return string.Empty;
      }

      var document = new HtmlDocument();
      document.LoadHtml(input);

      var nodes = new Queue<HtmlNode>(document.DocumentNode.SelectNodes("./*|./text()"));

      while (nodes.Count > 0)
      {
        var node = nodes.Dequeue();
        var parentNode = node.ParentNode;

        if (node.Name != "#text")
        {
          var childNodes = node.SelectNodes("./*|./text()");

          if (childNodes != null)
          {
            foreach (var child in childNodes)
            {
              nodes.Enqueue(child);
              parentNode.InsertBefore(child, node);
            }
          }
          parentNode.RemoveChild(node);
        }
      }

      return HttpUtility.HtmlDecode(document.DocumentNode.InnerHtml.Replace("&#160;", " "));
    }
  }
}
