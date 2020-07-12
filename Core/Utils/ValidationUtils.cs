using System;
using System.Text.RegularExpressions;

namespace Core.Utils
{
  [Flags]
  public enum ValidationCriteria
  {
    Required = 1,
    Length = 2,
    PhoneNumber = 4,
    Email = 8,
    Password = 16,
    ConfirmPassword = 32
  }

  public static class ValidationUtils
  {
    public static string ErrorRequiredFormat { get; set; } = "{0} is required";
    public static string ErrorLengthFormat { get; set; } = "{0} length should be between {1} - {2} chars";
    public static string ErrorInvalidNumberFormat { get; set; } = "{0} is not a valid phone number";
    public static string ErrorInvalidEmailFormat { get; set; } = "{0} is not a valid email address";
    public static string ErrorPasswordSimilarFormat { get; set; } = "{0} should not be similar to {1}";
    public static string ErrorPasswordsNotMatch { get; set; } = "Password and Confirmed password do no match.";

    public static bool Validate(string name,
                                string value,
                                ValidationCriteria criterias,
                                out string message,
                                int max = 0,
                                int min = 0,
                                object extra = null)
    {
      message = "";

      if (criterias.HasFlag(ValidationCriteria.Required))
      {
        if (string.IsNullOrEmpty(value))
        {
          message = string.Format(ErrorRequiredFormat, name);
          return false;
        }
      }

      if ((criterias & ValidationCriteria.Length) != 0)
      {
        if (value.Length > max || value.Length < min)
        {
          message = string.Format(ErrorLengthFormat, name, min, max);
          return false;
        }
      }

      if (criterias.HasFlag(ValidationCriteria.PhoneNumber))
      {
        var prefix = extra as string ?? "";

        if (!Regex.Match($"{prefix}{value}", @"^(\+[0-9]{3,15})$").Success)
        {
          message = string.Format(ErrorInvalidNumberFormat, name);
          return false;
        }
      }

      if (criterias.HasFlag(ValidationCriteria.Email))
      {
        if (string.IsNullOrEmpty(value))
        {
          return true;
        }

        try
        {
          var addr = new System.Net.Mail.MailAddress(value);

          if (addr.Address != value)
          {
            message = string.Format(ErrorInvalidEmailFormat, name);
            return false;
          }

        }
        catch
        {
          message = string.Format(ErrorInvalidEmailFormat, name);
          return false;
        }
      }

      if (criterias.HasFlag(ValidationCriteria.Password))
      {
        if (extra is Tuple<string,string> valueName)
        {
          if (!string.IsNullOrEmpty(valueName.Item1) && valueName.Item1.ToLower().Equals(value.ToLower()))
          {
            message = string.Format(ErrorPasswordSimilarFormat, name, valueName.Item2);
            return false;
          }
        }
      }

      if (criterias.HasFlag(ValidationCriteria.ConfirmPassword))
      {
        var cPassword = value as string;
        var password = extra as string;

        if (!cPassword.Equals(password))
        {
          message = ErrorPasswordsNotMatch;
          return false;
        }
      }

      return true;
    }
  }
}
