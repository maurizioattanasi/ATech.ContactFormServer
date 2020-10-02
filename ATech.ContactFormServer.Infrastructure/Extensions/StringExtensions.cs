using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace ATech.ContactFormServer.Infrastructure.Extensions
{

    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Check if the string is Null or Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Check if the string is Null or White Space
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// Check if the string contains only letters (No number or special characters)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsLetterlOnly(this string str)
        {
            return str.All(c => Char.IsLetter(c) || c == ' ' || c == '\'' || c == '.');
        }

        /// <summary>
        /// Verifies if the string content is a valid email
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmail(this string str)
        {
            return Regex.Match(str, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").Success;
        }

        /// <summary>
        /// Returns a MemoryStream from a given object
        /// </summary>
        /// <param name="str">The string to process</param>
        /// <param name="encoding">The desired encoding (default is UTF8)</param>
        /// <returns></returns>
        public static Stream ToStream(this string str, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return new MemoryStream(encoding.GetBytes(str ?? ""));
        }

        /// <summary>
        /// Replaces invalid XML characters in a string with their valid XML equivalent.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Escape(this string str)
        {
            return SecurityElement.Escape(str);
        }

        /// <summary>
        /// Prettyfies the given string 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Capitalize(this string str)
        {
            var tokens = str.Split('\'', StringSplitOptions.RemoveEmptyEntries);

            return tokens.Length switch
            {
                2 => $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tokens[0].ToLower())}\'{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tokens[1].ToLower())}",
                _ => $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower())}"
            };
        }

        /// <summary>
        /// Counts the number of words in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int CountWords(this string str)
            => str.IsNullOrEmpty() ? 0 : (str.Length - str.Replace(" ", string.Empty).Length + 1);

        /// <summary>
        /// Returns the first word in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstWord(this string str)
            => str.IsNullOrEmpty() ? str : str.Split(" ", StringSplitOptions.RemoveEmptyEntries).First();

        /// <summary>
        /// Returns the last word in a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string LastWord(this string str)
            => str.IsNullOrEmpty() ? str : str.Split(" ", StringSplitOptions.RemoveEmptyEntries).Last();
    }
}
