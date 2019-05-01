using System.Globalization;
using System.Text;

namespace NLemos.Api.Framework.Extensions
{
    public static class StringExtensions
    {
        public static string GenerateHash(this string text)
        {
            text = FormatString(text);
            text = text.Replace(" ", "").Replace(",", "").Replace("/", "").Replace("-", "");
            text = RemoveDiacritics(text);
            return text.Trim().ToLower();
        }

        private static string FormatString(string text)
        {
            if (text.StartsWith("\""))
            {
                text = text.Substring(1);
            }

            if (text.EndsWith("\""))
            {
                text = text.Substring(0, text.Length - 1);
            }

            return text.Trim();
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}