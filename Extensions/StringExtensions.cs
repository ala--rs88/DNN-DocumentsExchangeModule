using System;

namespace IgorKarpov.DocumentsExchangeModule.Extensions
{
    public static class StringExtensions
    {
        public static String Shorten(this String stringToShorten)
        {
            return stringToShorten.Length <= 30
                       ? stringToShorten :
                       String.Format("{0}...", stringToShorten.Substring(0, 28).TrimEnd());
        }
    }
}