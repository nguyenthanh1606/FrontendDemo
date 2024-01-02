using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Frontend.Infrastructure.ExtensionMethod
{
    public static class ExtensionMethod
    {
        const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static string Truncate(this string value, int maxChars)
        {
            if (value == null || value.Length < maxChars || value.IndexOf(" ", maxChars) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", maxChars)) + "...";
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if(string.IsNullOrEmpty(source))
            {
                return false;
            }
            if(string.IsNullOrEmpty(toCheck))
            {
                return true;
            }
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string RemoveHtml(this string value)
        {
            if(value!=null)
            {
                string strRegex = @"</{0,1}(!DOCTYPE|a|abbr|acronym|address|applet|area|article|aside|audio|b|base|basefont|bdi|bdo|big|blockquote|body|br|button|canvas|caption|center|cite|code|col|colgroup|datalist|dd|del|details|dfn|dialog|dir|div|dl|dt|em|embed|fieldset|figcaption|figure|font|footer|form|frame|frameset|h1|h2|h3|h4|h5|h6|head|header|hr|html|i|iframe|img|input|ins|kbd|keygen|label|legend|li|link|main|map|mark|menu|menuitem|meta|meter|nav|noframes|noscript|object|ol|optgroup|option|output|p|param|pre|progress|q|rp|rt|ruby|s|samp|script|section|select|small|source|span|strike|strong|style|sub|summary|sup|table|tbody|td|textarea|tfoot|th|thead|time|title|tr|track|tt|u|ul|var|video|wbr){1}(\s*/{0,1}>|\s+.*?/{0,1}>)";
                Regex myRegex = new Regex(strRegex, RegexOptions.Singleline);
                string strReplace = string.Empty;
                return myRegex.Replace(value, strReplace);
            }

            return value;
        }
        public static string RemoveTagP(this string value)
        {
            if (value != null)
            {
                string ans = value.Replace("<p>","");
                value = ans.Replace("</p>", "");
                return value;
            }
            return value;
        }


        public static string RemoveLinkImage(this string value)
        {
            if (value != null)
            {
                value = value.Remove(value.IndexOf("style"), 36);
                return value;
            }
            return value;
        }
        public static char ToAlphabet(this int value)
        {
            return Alphabet[value];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string[] SplitToArrayDigit(this int value)
        {
            var numbers = new Stack<string>();
            if (value < 0)
            {
                numbers.Push("-");
                value = -value;
            }
            while (value > 0)
            {
                numbers.Push((value % 10).ToString());
                value /= 10;
            }

            return numbers.ToArray();
        }

        public static string[] SplitToArrayDigit(this string srt)
        {
            int value;
            if(int.TryParse(srt, out value))
            {
                var numbers = new Stack<string>();
                if (value < 0)
                {
                    numbers.Push("-");
                    value = -value;
                }
                while (value > 0)
                {
                    numbers.Push((value % 10).ToString());
                    value /= 10;
                }

                return numbers.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Find first value of identity by type claims
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetIdentityValue(this IIdentity identity, string type)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(type);
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static T GetIdentityValue<T>(this IIdentity identity, string type) where T:IConvertible
        {
            var claim = ((ClaimsIdentity)identity).FindFirst(type);
            // Test for null to avoid issues during local testing
            return (claim != null) ? (T)Convert.ChangeType(claim.Value, typeof(T)) : default(T);
        }

        // Extension for Identity
        public static List<string> GetIdentityValues(this IIdentity identity, string type)
        {
            var claim = ((ClaimsIdentity)identity).FindAll(type).Select(t=>t.Value).ToList();
            // Test for null to avoid issues during local testing
            return claim;
        }

        /// <summary>
        /// Extension for IPrincipal --- "DocumentRole" follow LoginController/IdentitySignin
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool IsInAnyDocumentRole(this IPrincipal user, string role)
        {
            var userRoles = user.Identity.GetIdentityValues("DocumentRole");
            return userRoles.Any(u => u == role);
        }

        public static string GetDepartment(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Department");
            return (claim != null) ? claim.Value : string.Empty;
        }


        //public static string ToLanguageText(this string value)
        //{
        //    switch(value)
        //    {
        //        case "vi-VN":
        //            return LocalResource.Resource.Vietnamese;
        //        case "en-US":
        //            return LocalResource.Resource.English;
        //        default:
        //            return null;
        //    }
        //}

        public static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(System.Text.NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            Regex r = new Regex("(?:[^a-z0-9- ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            string url = (sb.ToString().Normalize(NormalizationForm.FormC)).Trim().Replace(" ", "-").ToLower().Replace("đ", "d");
            return r.Replace(url, String.Empty);
        }

        public static string ToPercentageString(this double value)
        {
            return string.Format("{0}%", value * 100);
        }

        public static double ToPercentage(this double value)
        {
            return value * 100;
        }
    }
}