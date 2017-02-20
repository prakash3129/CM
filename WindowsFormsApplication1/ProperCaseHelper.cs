using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace GCC
{
    public static class ProperCaseHelper
    {
        public static DataTable dtProperCase = new DataTable();
        public static string NameANDJobTitleCasing(string input,string sProperCasingTo)
        {
         //   if (IsAllUpperOrAllLower(input))
            {
                // fix the ALL UPPERCASE or all lowercase names
                return string.Join(" ", input.Split(new Char [] {' '}).Select(word => wordToProperCase(word, sProperCasingTo)));
            }
            //else
            //{
            //    // leave the CamelCase or Propercase names alone
            //    return input;
            //}
        }

        public static bool IsAllUpperOrAllLower(string input)
        {
            return (input.ToLower().Equals(input) || input.ToUpper().Equals(input));
        }

        private static string wordToProperCase(string word, string sProperCasingTo)
        {
            if (string.IsNullOrEmpty(word)) return word;
            
            // Standard case
            string ret = string.Empty;

            if (word.ToUpper() == word)
                ret = word;
            else
                ret = capitaliseFirstLetter(word.Replace(".", string.Empty)); //remove Dots

            ret = dealWithRomanNumerals(ret);   // William Gates, III


            DataRow[] drSelected = null;
            if (dtProperCase.Select("picklistField = '" + sProperCasingTo + "'").Length > 0)
                drSelected = dtProperCase.Select("picklistField = '" + sProperCasingTo + "'");
            else
                return ret;//Only propercase.. No spl replacement

            foreach (DataRow dr in drSelected)
            {
                switch (dr["Remarks"].ToString().ToUpper())
                {
                    case "PROPERSUFFIX":
                        ret = properSuffix(ret, dr["picklistvalue"].ToString());
                        break;
                    case "PROPERSUFFIXBEGINS":
                        ret = properSuffixBegins(ret, dr["picklistvalue"].ToString());
                        break;
                    case "SPECIALWORDS":
                        ret = specialWords(ret, dr["picklistvalue"].ToString());
                        break;
                    case "UPPERCASE":
                        ret = UpperCase(ret, dr["picklistvalue"].ToString());
                        break;
                }
            }
            //// Special cases:
            //ret = properSuffix(ret, "'");   // D'Artagnon, D'Silva
            //ret = properSuffix(ret, ".");   // ???
            //ret = properSuffix(ret, "-");       // Oscar-Meyer-Weiner            
            //ret = properSuffix(ret, "Mc");      // McDonald
            //ret = properSuffix(ret, "Mac");     // Scots

            //// Special words:
            //ret = specialWords(ret, "van");     // Dick van Dyke
            //ret = specialWords(ret, "von");     // Baron von Bruin-Valt
            //ret = specialWords(ret, "de");
            //ret = specialWords(ret, "du");
            //ret = specialWords(ret, "di");
            //ret = specialWords(ret, "le");
            //ret = specialWords(ret, "da");      // Leonardo da Vinci, Eduardo da Silva


            //ret = specialWords(ret, "of");      // The Grand Old Duke of York
            
            //ret = specialWords(ret, "HRH");     // His/Her Royal Highness
            //ret = specialWords(ret, "HRM");     // His/Her Royal Majesty
            //ret = specialWords(ret, "H.R.H.");  // His/Her Royal Highness
            //ret = specialWords(ret, "H.R.M.");  // His/Her Royal Majesty

            //le

            return ret;
        }

        private static string UpperCase(string word, string jobtitle)
        {
            if (string.IsNullOrEmpty(word)) return word;

            if (word.ToUpper().Equals(jobtitle.ToUpper()))
                return word.ToUpper();
            else
                return word;
        }

        private static string properSuffixBegins(string word, string prefix)
        {
            if (string.IsNullOrEmpty(word)) return word;

            if (word.ToUpper().StartsWith(prefix.ToUpper()))
            {
                word = ("~" + word).ToLower().Replace("~" + prefix.ToLower(), string.Empty);
                word = capitaliseFirstLetter(word);
                return prefix + word;
            }

            else
                return word;
        }

        private static string properSuffix(string word, string prefix)
        {
            if (string.IsNullOrEmpty(word)) return word;

            string lowerWord = word.ToLower();
            string lowerPrefix = prefix.ToLower();

            if (!lowerWord.Contains(lowerPrefix)) return word;

            int index = lowerWord.IndexOf(lowerPrefix);

            // If the search string is at the end of the word ignore.
            if (index + prefix.Length == word.Length) return word;

            return word.Substring(0, index) + prefix +
                capitaliseFirstLetter(word.Substring(index + prefix.Length));
        }

        private static string specialWords(string word, string specialWord)
        {
            if (word.Equals(specialWord, StringComparison.InvariantCultureIgnoreCase))
                return specialWord;
            else
                return word;
        }

        private static string dealWithRomanNumerals(string word)
        {
            List<string> ones = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            List<string> tens = new List<string>() { "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC", "C" };
            // assume nobody uses hundreds

            foreach (string number in ones)
            {
                if (word.Equals(number, StringComparison.InvariantCultureIgnoreCase))
                {
                    return number;
                }
            }

            foreach (string ten in tens)
            {
                foreach (string one in ones)
                {
                    if (word.Equals(ten + one, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return ten + one;
                    }
                }
            }

            return word;
        }

        private static string capitaliseFirstLetter(string word)
        {
            if (string.IsNullOrEmpty(word)) return word;
            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }
    }
}
