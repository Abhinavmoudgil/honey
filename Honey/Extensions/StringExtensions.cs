namespace Honey.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Web.UI;

    public static class StringExtensions
    {
        public static string RegexSafe(this string source)
        {
            return source.Replace("(", "\\(").Replace(")", "\\)").Replace("+", "\\+").Replace("*", "\\*").Replace("?", "\\?").Replace("[", "\\[").Replace("]", "\\]").Replace("$", "\\$").Replace("^", "\\^").Replace(".", "\\.").Replace("{", "\\{").Replace("}", "\\}").Replace("\\", "\\\\").Replace("|", "\\|").Replace(">", "\\>").Replace("<", "\\<");
        }

        public static string FormatWith(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static string FormatWith(this string format, object source)
        {
            return StringExtensions.FormatWith(format, (IFormatProvider)null, source);
        }

        public static string FormatWith(this string format, IFormatProvider provider, object source)
        {
            if (format == null)
                throw new ArgumentNullException("format");
            Regex regex = new Regex("(?<start>\\{)+(?<property>[\\w\\.\\[\\]]+)(?<format>:[^}]+)?(?<end>\\})+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);
            List<object> values = new List<object>();
            string format1 = regex.Replace(format, (MatchEvaluator)(m =>
            {
                Group local_0 = m.Groups["start"];
                Group local_1 = m.Groups["property"];
                Group local_2 = m.Groups["format"];
                Group local_3 = m.Groups["end"];
                values.Add(local_1.Value == "0" ? source : DataBinder.Eval(source, local_1.Value));
                return string.Concat(new object[4]
        {
          (object) new string('{', local_0.Captures.Count),
          (object) (values.Count - 1),
          (object) local_2.Value,
          (object) new string('}', local_3.Captures.Count)
        });
            }));
            return string.Format(provider, format1, values.ToArray());
        }

        public static string Normalise(this string value)
        {
            return Regex.Replace(value, "\\s", string.Empty).Trim();
        }

        public static string SurroundWith(this string value, char tag)
        {
            return string.Format("{0}{1}{2}", (object)tag, (object)value, (object)tag);
        }

        public static string RegexReplace(this string source, string pattern, string replacement)
        {
            return Regex.Replace(source, pattern, replacement);
        }

        public static string ToTitleCase(this string str)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToTitleCase(this string str, string cultureInfoName)
        {
            return new CultureInfo(cultureInfoName).TextInfo.ToTitleCase(str.ToLower());
        }

        public static string ToTitleCase(this string str, CultureInfo cultureInfo)
        {
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }

        public static List<string> SplitOn(this string initial, int maxCharacters)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(initial))
            {
                string index = "Line";
                string pattern = string.Format("(?<{0}>.{{1,{1}}})(?:\\W|$)", (object)index, (object)maxCharacters);
                MatchCollection matchCollection = Regex.Matches(initial, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.CultureInvariant);
                if (matchCollection != null && matchCollection.Count > 0)
                {
                    foreach (Match match in matchCollection)
                        list.Add(match.Groups[index].Value);
                }
            }
            return list;
        }

        public static string Join<T>(this IEnumerable<T> collection, string seperator)
        {
            return string.Join<T>(seperator, collection);
        }

        public static bool HasValue(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static int LevenshteinDistance(this string input, string comparedTo, bool caseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(comparedTo))
                return -1;
            if (!caseSensitive)
            {
                input = input.ToLower();
                comparedTo = comparedTo.ToLower();
            }
            int length1 = input.Length;
            int length2 = comparedTo.Length;
            int[,] numArray = new int[length1, length2];
            for (int index = 0; index < length1; ++index)
                numArray[index, 0] = index;
            for (int index = 0; index < length2; ++index)
                numArray[0, index] = index;
            for (int index1 = 1; index1 < length1; ++index1)
            {
                char ch1 = input[index1 - 1];
                for (int index2 = 1; index2 < length2; ++index2)
                {
                    char ch2 = comparedTo[index2 - 1];
                    int num1 = (int)ch1 == (int)ch2 ? 0 : 1;
                    int num2 = StringExtensions.FindMinimum(numArray[index1 - 1, index2] + 1, numArray[index1, index2 - 1] + 1, numArray[index1 - 1, index2 - 1] + num1);
                    if (index1 > 1 && index2 > 1)
                    {
                        int num3 = numArray[index1 - 2, index2 - 2] + 1;
                        if ((int)input[index1 - 2] != (int)comparedTo[index2 - 1])
                            ++num3;
                        if ((int)input[index1 - 1] != (int)comparedTo[index2 - 2])
                            ++num3;
                        if (num2 > num3)
                            num2 = num3;
                    }
                    numArray[index1, index2] = num2;
                }
            }
            return numArray[length1 - 1, length2 - 1];
        }

        private static int FindMinimum(params int[] p)
        {
            if (null == p)
                return int.MinValue;
            int num = int.MaxValue;
            for (int index = 0; index < p.Length; ++index)
            {
                if (num > p[index])
                    num = p[index];
            }
            return num;
        }

        public static Tuple<string, double> LongestCommonSubsequence(this string input, string comparedTo, bool caseSensitive = false)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(comparedTo))
                return new Tuple<string, double>(string.Empty, 0.0);
            if (!caseSensitive)
            {
                input = input.ToLower();
                comparedTo = comparedTo.ToLower();
            }
            int length1 = input.Length;
            int length2 = comparedTo.Length;
            int[,] numArray1 = new int[length1 + 1, length2 + 1];
            StringExtensions.LcsDirection[,] lcsDirectionArray = new StringExtensions.LcsDirection[length1 + 1, length2 + 1];
            int[,] numArray2 = new int[length1 + 1, length2 + 1];
            for (int index = 0; index <= length1; ++index)
            {
                numArray1[index, 0] = 0;
                lcsDirectionArray[index, 0] = StringExtensions.LcsDirection.North;
            }
            for (int index = 0; index <= length2; ++index)
            {
                numArray1[0, index] = 0;
                lcsDirectionArray[0, index] = StringExtensions.LcsDirection.West;
            }
            for (int index1 = 1; index1 <= length1; ++index1)
            {
                for (int index2 = 1; index2 <= length2; ++index2)
                {
                    if (input[index1 - 1].Equals(comparedTo[index2 - 1]))
                    {
                        int p = numArray2[index1 - 1, index2 - 1];
                        numArray1[index1, index2] = numArray1[index1 - 1, index2 - 1] + StringExtensions.Square(p + 1) - StringExtensions.Square(p);
                        lcsDirectionArray[index1, index2] = StringExtensions.LcsDirection.NorthWest;
                        numArray2[index1, index2] = p + 1;
                    }
                    else
                    {
                        numArray1[index1, index2] = numArray1[index1 - 1, index2 - 1];
                        lcsDirectionArray[index1, index2] = StringExtensions.LcsDirection.None;
                    }
                    if (numArray1[index1 - 1, index2] >= numArray1[index1, index2])
                    {
                        numArray1[index1, index2] = numArray1[index1 - 1, index2];
                        lcsDirectionArray[index1, index2] = StringExtensions.LcsDirection.North;
                        numArray2[index1, index2] = 0;
                    }
                    if (numArray1[index1, index2 - 1] >= numArray1[index1, index2])
                    {
                        numArray1[index1, index2] = numArray1[index1, index2 - 1];
                        lcsDirectionArray[index1, index2] = StringExtensions.LcsDirection.West;
                        numArray2[index1, index2] = 0;
                    }
                }
            }
            int index3 = length1;
            int index4 = length2;
            string str = "";
            double num1 = (double)numArray1[index3, index4];
            while (index3 > 0 || index4 > 0)
            {
                if (lcsDirectionArray[index3, index4] == StringExtensions.LcsDirection.NorthWest)
                {
                    --index3;
                    --index4;
                    str = (string)(object)input[index3] + (object)str;
                }
                else if (lcsDirectionArray[index3, index4] == StringExtensions.LcsDirection.North)
                    --index3;
                else if (lcsDirectionArray[index3, index4] == StringExtensions.LcsDirection.West)
                    --index4;
            }
            double num2 = num1 / (double)(length1 * length2);
            return new Tuple<string, double>(str, num2);
        }

        private static int Square(int p)
        {
            return p * p;
        }  

        public static string NoSpaces(this string value)
        {
            return StringExtensions.HasValue(value) ? value.Replace(" ", string.Empty) : value;
        }

        internal enum LcsDirection
        {
            None,
            North,
            West,
            NorthWest,
        }
    }
}