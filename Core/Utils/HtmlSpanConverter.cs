using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Core.Utils
{
  public class HtmlSpanConverter : IValueConverter
  {
    readonly ICommand _command;

    public HtmlSpanConverter()
    {
      _command = new Command<string>(OpenLink);
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var formatted = new FormattedString();

      if (   value == null
          || !(value is string s)
          || string.IsNullOrEmpty(s.Trim()))
      {
        return value;
      }

      foreach (var item in ProcessString(s))
      {
        var span = CreateSpan(item);
        if (span == null)
        {
          continue;
        }
        formatted.Spans.Add(span);
      }

      // Hack This is a hack where tap gesture not working in iOS for spans at end of the line. So to fix the issue,
      // adds an empty new line with tap gesture and filter open link method
      var sp = new Span { Text = " \n " };
      sp.GestureRecognizers.Add(new TapGestureRecognizer
      {
        Command = _command,
        CommandParameter = ""
      });

      formatted.Spans.Add(sp);

      return formatted;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private Span CreateSpan(StringSection section)
    {
      var text = section.Text.StripHtml()?.Trim();
      if (string.IsNullOrEmpty(text))
      {
        return null;
      }

      text = Regex.Replace(section.Text, "<br.*?>|<.?p>", "\n")
                  .StripHtml();

      var span = new Span
      {
        Text = text
      };

      if (!string.IsNullOrEmpty(section.Link))
      {
        span.GestureRecognizers.Add(new TapGestureRecognizer()
        {
          Command = new Command<string>(OpenLink),
          CommandParameter = section.Link
        });
        span.TextColor = Color.FromHex("#0072bc");
        span.TextDecorations = TextDecorations.Underline;
      }

      return span;
    }

    IList<StringSection> ProcessString(string rawText)
    {
      const string spanPattern = @"(<[abi].*?>.*?</[abi]>)";

      MatchCollection collection = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline);

      var sections = new List<StringSection>();

      var lastIndex = 0;

      foreach (Match item in collection)
      {
        var foundText = item.Value;

        sections.Add(new StringSection { Text = rawText.Substring(lastIndex, item.Index - lastIndex) });

        lastIndex += (item.Index - lastIndex) + item.Length;

        // Get HTML href 
        var html = new StringSection
        {
          Link = Regex.Match(item.Value, "(?<=href=\\\")[\\S]+(?=\\\")").Value,
          Text = $" {item.Value} "
        };

        sections.Add(html);
      }

      sections.Add(new StringSection { Text = rawText.Substring(lastIndex) });

      return sections;
    }

    void OpenLink(string url)
    {
      if (!string.IsNullOrEmpty(url))
      {
        Browser.OpenAsync(url);
      }
    }

    class StringSection
    {
      public string Text { get; set; }
      public string Link { get; set; }
    }


  }
}
