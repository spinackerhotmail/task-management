using System.Text.RegularExpressions;

namespace TaskManagementService.CommonLib.Extentions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string o) =>
               Regex.Replace(o, @"(\w)([A-Z])", "$1_$2").ToLower();

        public static string ToCamelCase(this string str) =>
             string.IsNullOrEmpty(str) || str.Length < 2
             ? str.ToLowerInvariant()
             : char.ToLowerInvariant(str[0]) + str.Substring(1);

        public static string ClearText(this string? text)
        {
            return Regex.Replace(text ?? string.Empty, @"\s+", "").Trim();
        }
    }
}
