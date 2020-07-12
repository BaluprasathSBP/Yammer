using System.Text;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Core.Droid.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Core.Droid.Renderers
{
  public static class FormattedStringExtensions
  {
    public static SpannableString ToAttributed(this FormattedString formattedString,
                                               Android.Content.Context context,
                                               FontAttributes attr,
                                               Xamarin.Forms.Color defaultForegroundColor,
                                               double textSize)
    {
      if (formattedString == null)
      {
        return null;
      }

      var builder = new StringBuilder();
      for (int i = 0; i < formattedString.Spans.Count; i++)
      {
        Span span = formattedString.Spans[i];
        var text = span.Text;
        if (text == null)
        {
          continue;
        }

        builder.Append(text);
      }

      var spannable = new SpannableString(builder.ToString());

      var c = 0;
      for (int i = 0; i < formattedString.Spans.Count; i++)
      {
        Span span = formattedString.Spans[i];
        var text = span.Text;
        if (text == null)
        {
          continue;
        }

        int start = c;
        int end = start + text.Length;
        c = end;

        if (span.TextColor != Xamarin.Forms.Color.Default)
        {
          spannable.SetSpan(new ForegroundColorSpan(span.TextColor.ToAndroid()),
                                                    start,
                                                    end,
                                                    SpanTypes.InclusiveExclusive);
        }
        else if (defaultForegroundColor != Xamarin.Forms.Color.Default)
        {
          spannable.SetSpan(new ForegroundColorSpan(defaultForegroundColor.ToAndroid()),
                                                   start,
                                                   end,
                                                   SpanTypes.InclusiveExclusive);
        }

        if (span.BackgroundColor != Xamarin.Forms.Color.Default)
        {
          spannable.SetSpan(new BackgroundColorSpan(span.BackgroundColor.ToAndroid()),
                                                    start,
                                                    end,
                                                    SpanTypes.InclusiveExclusive);
        }

        spannable.SetSpan(new TypefaceSpan(
          context,
          TypefaceFactory.GetTypefaceForFontAttribute(context, attr), textSize),
          start,
          end,
          SpanTypes.InclusiveInclusive);
      }
      return spannable;
    }

    class TypefaceSpan : MetricAffectingSpan
    {
      readonly Android.Content.Context _context;
      readonly double _textSize;
      readonly Typeface _typeface;

      public TypefaceSpan(Android.Content.Context context, Typeface typeface, double textSize)
      {
        _context = context;
        _typeface = typeface;
        _textSize = textSize;
      }

      public override void UpdateDrawState(TextPaint tp)
      {
        Apply(tp);
      }

      public override void UpdateMeasureState(TextPaint p)
      {
        Apply(p);
      }

      void Apply(Paint paint)
      {
        paint.SetTypeface(_typeface);
        paint.TextSize = TypedValue.ApplyDimension(ComplexUnitType.Dip,
                                                   (float)_textSize,
                                                   _context.Resources.DisplayMetrics);
      }
    }
  }
}
