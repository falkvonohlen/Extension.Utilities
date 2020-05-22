using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Extension.Utilities.ClassExtensions
{
    /// <summary>
    /// Extensions for strings
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Makes sure a string which represents a relative path, is formatted correctly for .net
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string MakeDotNetRelativePath(this string instance)
        {
            return instance.Replace("/", "\\").TrimStart('\\');
        }

        /// <summary>
        /// Checks, if the string already exists in the collection of
        /// known values and iterates it until it is unique within the collection
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="knownValues"></param>
        /// <returns></returns>
        public static string MakeCaseUnique(this string instance, IEnumerable<string> knownValues)
        {
            if (knownValues.Any(v => instance.Equals(v, StringComparison.InvariantCultureIgnoreCase)))
            {
                return instance.IterateNumericTail().MakeCaseUnique(knownValues);
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Checks, if the string already exists in the collection of
        /// known values and iterates it until it is unique within the collection
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="knownValues"></param>
        /// <returns></returns>
        public static string MakeCaseUnique(this string instance, IEnumerable<string> knownValues, string seperator)
        {
            if (knownValues.Any(v => instance.Equals(v, StringComparison.InvariantCultureIgnoreCase)))
            {
                return instance.IterateNumericTail(seperator).MakeCaseUnique(knownValues);
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Check if the value exists within this string by using a culture case ignore compare
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string instance, string value)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(instance, value, CompareOptions.IgnoreCase) != -1;
        }

        /// <summary>
        /// Checks, if the string already exists in the collection of known 
        /// values and iterates it until it is unique within the collection
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="knownValues"></param>
        /// <returns></returns>
        public static string MakeUnique(this string instance, IEnumerable<string> knownValues)
        {
            if (knownValues.Contains(instance))
            {
                return instance.IterateNumericTail().MakeUnique(knownValues);
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Checks, if the string already exists in the collection of known 
        /// values and iterates it until it is unique within the collection
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="knownValues"></param>
        /// <returns></returns>
        public static string MakeUnique(this string instance, IEnumerable<string> knownValues, string seperator)
        {
            if (knownValues.Contains(instance))
            {
                return instance.IterateNumericTail(seperator).MakeUnique(knownValues);
            }
            else
            {
                return instance;
            }
        }

        /// <summary>
        /// Iterates the numeric tail of a string
        /// or adds one, if no numeric tail exists
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateNumericTail(this string instance)
        {
            var numTail = instance.GetNumericTail();
            var noneIterated = instance.TrimEnd(numTail.ToCharArray());
            if (string.IsNullOrEmpty(numTail))
            {
                return instance + "1";
            }
            else
            {
                if (int.TryParse(numTail, out int tail))
                {
                    return noneIterated + (tail + 1);
                }
                else
                {
                    throw new InvalidCastException($"Could not parse the numeric tail {numTail} to an integer");
                }
            }
        }

        /// <summary>
        /// Iterates the numeric tail of a string which needs to be seperated with a provided seperator
        /// or adds one, if no numeric tail exists
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateNumericTail(this string instance, string seperator)
        {
            var numTail = instance.GetNumericTail(seperator);
            var noneIterated = instance.TrimEnd(numTail.ToCharArray());
            if (string.IsNullOrEmpty(numTail))
            {
                return instance.AttachSeperator(seperator) + "1";
            }
            else
            {
                if (int.TryParse(numTail, out int tail))
                {
                    return noneIterated.AttachSeperator(seperator) + (tail + 1);
                }
                else
                {
                    throw new InvalidCastException($"Could not parse the numeric tail {numTail} to an integer");
                }
            }
        }

        /// <summary>
        /// Attaches a seperator to the end of the string, or leave the string
        /// untouched, if it already ends with an underscore
        /// </summary>
        /// <returns></returns>
        private static string AttachSeperator(this string instance, string seperator)
        {
            if (instance.EndsWith(seperator))
            {
                return instance;
            }
            else
            {
                return instance + seperator;
            }
        }

        /// <summary>
        /// Returns the numeric tail of a string value.
        /// E.g Hallo123 will return 123 as a string
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetNumericTail(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return string.Empty;
            }

            var leftOver = instance.Remove(instance.Length - 1);
            var lastChar = instance.Last();
            if (char.IsNumber(lastChar))
            {
                return leftOver.GetNumericTail() + lastChar;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the numeric tail of a string value after the last occurance of the given seperator
        /// E.g Hallo_123 will return 123 as a string, but HalloWelt123 will return an empty string
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetNumericTail(this string instance, string seperator)
        {
            var end = instance.Split(seperator.ToCharArray()).LastOrDefault();
            if (string.IsNullOrEmpty(end) || instance == end)
            {
                return string.Empty;
            }
            else
            {
                return end.GetNumericTail();
            }
        }

        /// <summary>
        /// Get a substring from the end of the string which only consist of upper case letters
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetUpperLetterTail(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return string.Empty;
            }

            var leftOver = instance.Remove(instance.Length - 1);
            var lastChar = instance.Last();
            if (char.IsLetter(lastChar) && !char.IsLower(lastChar)) 
            {
                return leftOver.GetUpperLetterTail() + lastChar;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get a substring from the end of the string which only consist of lower case letters
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string GetLowerLetterTail(this string instance)
        {
            if (string.IsNullOrEmpty(instance))
            {
                return string.Empty;
            }

            var leftOver = instance.Remove(instance.Length - 1);
            var lastChar = instance.Last();
            if (char.IsLetter(lastChar) && char.IsLower(lastChar))
            {
                return leftOver.GetLowerLetterTail() + lastChar;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Iterates the upper letter tail of the current string
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateUpperLetterTail(this string instance)
        {
            var tail = instance.GetUpperLetterTail();
            var body = instance.Remove(instance.Length - tail.Length);
            return body + tail.IterateUpperLetter();
        }

        /// <summary>
        /// Iterates the upper letter tail of the current string
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateLowerLetterTail(this string instance)
        {
            var tail = instance.GetLowerLetterTail();
            var body = instance.Remove(instance.Length - tail.Length);
            return body + tail.IterateLowerLetter();
        }

        /// <summary>
        /// Iterates the string like a numeric value
        /// AA -> AB
        /// BCD -> BCE
        /// Z -> AA
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateUpperLetter(this string instance)
        {
            instance = instance.ToUpper();
            var constantPart = instance.Remove(instance.Length - 1);
            var iterated = instance.GetIteratedLastLetterUpper();
            if (iterated == default)
            {
                if (constantPart.Length == 0)
                {
                    return "AA";
                }
                else
                {
                    return constantPart.IterateUpperLetter() + "A";
                }
            }
            else
            {
                return constantPart + iterated;
            }
        }

        /// <summary>
        /// Iterates the string like a numeric value
        /// AA -> Z
        /// BCD -> BCC
        /// Z -> Y
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ReverseIterateUpperLetter(this string instance)
        {
            instance = instance.ToUpper();
            var constantPart = instance.Remove(instance.Length - 1);
            var reverseIterated = instance.GetReverseIteratedLastLetterUpper();
            if (reverseIterated == default)
            {
                if (constantPart.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return constantPart.ReverseIterateUpperLetter() + "Z";
                }
            }
            else
            {
                return constantPart + reverseIterated;
            }
        }

        /// <summary>
        /// Iterates the string like a numeric value
        /// aa -> ab
        /// bcd -> bce
        /// z -> aa
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string IterateLowerLetter(this string instance)
        {
            instance = instance.ToLower();
            var constantPart = instance.Remove(instance.Length - 1);
            var iterated = instance.GetIteratedLastLetterLower();
            if (iterated == default)
            {
                if (constantPart.Length == 0)
                {
                    return "aa";
                }
                else
                {
                    return constantPart.IterateLowerLetter() + "a";
                }
            }
            else
            {
                return constantPart + iterated;
            }
        }

        /// <summary>
        /// Iterates the string like a numeric value
        /// aa -> z
        /// bcd -> bcc
        /// z -> y
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static string ReverseIterateLowerLetter(this string instance)
        {
            instance = instance.ToLower();
            var constantPart = instance.Remove(instance.Length - 1);
            var reverseIterated = instance.GetReverseIteratedLastLetterLower();
            if (reverseIterated == default)
            {
                if (constantPart.Length == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return constantPart.ReverseIterateLowerLetter() + "z";
                }
            }
            else
            {
                return constantPart + reverseIterated;
            }
        }

        /// <summary>
        /// Gets the next char in the english alphabet after the currently last char of the string
        /// or an empty string, if already fully iterated
        /// Hallo -> P
        /// Test -> U
        /// bgdz -> default
        /// </summary>
        /// <returns></returns>
        public static char GetIteratedLastLetterUpper(this string instance)
        {
            var last = instance.ToUpper().LastOrDefault();
            if (last == default || last == 'Z')
            {
                return default;
            }
            else
            {
                last++;
                return last;
            }
        }

        /// <summary>
        /// Gets the next char in the english alphabet after the currently last char of the string
        /// or an empty string, if already fully iterated
        /// Hallo -> N
        /// Test -> S
        /// bgda -> default
        /// </summary>
        /// <returns></returns>
        public static char GetReverseIteratedLastLetterUpper(this string instance)
        {
            var last = instance.ToUpper().LastOrDefault();
            if (last == default || last == 'A')
            {
                return default;
            }
            else
            {
                last--;
                return last;
            }
        }

        /// <summary>
        /// Gets the next char in the english alphabet after the currently last char of the string
        /// or an empty string, if already fully iterated
        /// Hallo -> p
        /// Test -> u
        /// bgdz -> default
        /// </summary>
        /// <returns></returns>
        public static char GetIteratedLastLetterLower(this string instance)
        {
            var last = instance.ToLower().LastOrDefault();
            if (last == default || last == 'z')
            {
                return default;
            }
            else
            {
                last++;
                return last;
            }
        }

        /// <summary>
        /// Gets the next char in the english alphabet after the currently last char of the string
        /// or an empty string, if already fully iterated
        /// Hallo -> n
        /// Test -> s
        /// bgda -> default
        /// </summary>
        /// <returns></returns>
        public static char GetReverseIteratedLastLetterLower(this string instance)
        {
            var last = instance.ToLower().LastOrDefault();
            if (last == default || last == 'a')
            {
                return default;
            }
            else
            {
                last--;
                return last;
            }
        }

    }
}
