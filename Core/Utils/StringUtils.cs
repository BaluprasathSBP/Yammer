namespace Core.Utils
{
  public static class StringUtils
  {
    public static bool IsPunctuation(this string value)
    {
      switch (value)
      {
        case "!":
        case "\"":
        case "#":
        case "%":
        case "&":
        case "\\":
        case "(":
        case ")":
        case "*":
        case ",":
        case "-":
        case ".":
        case "/":
        case ":":
        case ";":
        case "?":
        case "@":
        case "[":
        case "\\\\":
        case "]":
        case "_":
        case "{":
        case "}":
        case "•":
          return true;
        default:
          return false;
      }
    }
  }
}
