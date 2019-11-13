namespace Defra.Imports.BusinessLogic.Extensions
{
    using System.Text;

    public static class StringExtensions
    {
        public static string ReplaceFromPosition(this string s, int index, int length, string replacement)
        {
            var builder = new StringBuilder();
            builder.Append(s.Substring(0, index));
            builder.Append(replacement);
            builder.Append(s.Substring(index + length));
            return builder.ToString();
        }
    }
}
