using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RhymeDictionary.Model.Extensions
{
    public static class CharExtensions
    {
        public static bool IsBulgarianVowel(this char c)
        {
            return "аъоуеиюяАЪОУЕИЮЯ".Contains(c);
        }

        public static bool ContainsUpperCaseVowel(this string s)
        {
            return s.IndexOfAny("АЪОУЕИЮЯ".ToCharArray()) != -1;
        }

        public static bool ContainsStress(this string s)
        {
            return s.IndexOf("'") != -1;
        }

        public static string CoerceToLowerCaseStressed(this string s)
        {
            string upperCaseVowelsTurnedToStress = new Regex("[АЪОУЕИЮЯ]").Replace(s, "'$0");

            var result = upperCaseVowelsTurnedToStress.ToLower();
            if (!result.ContainsStress())
            {
                throw new ArgumentException("Could not coerce word. Make sure it contains an uppercase vowel or apostrophy stress.");
            }

            return result;
        }
    }
}
