using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public enum ECase
{
    NONE, UPPERCASE, LOWERCASE, CAPITALIZE
}

namespace ExtensionMethods
{
    public static class StringUtils
    {
        // https://stackoverflow.com/a/18867218/3399416
        public static string FormatCase(this string str, ECase c){
            switch(c){
                case ECase.UPPERCASE: return str.ToUpper();
                case ECase.LOWERCASE: return str.ToLower();
                case ECase.CAPITALIZE: return str.Substring(0,1).ToUpper() + str.Substring(1).ToLower();
                default: return str;
            }
        }

        public static List<T> ParseList<T>(string str, Func<string, T> parsingMethod){
            if (string.IsNullOrEmpty(str)) {
                return new List<T>();
            } else {
                return str
                    .Split(',')
                    .Select(s => parsingMethod(s))
                    .ToList();
            }
        }

        public static List<int> ParseIntList(string str)
            => ParseList(str, int.Parse);

        public static string ReplaceAt(this string input, int index, char newChar){
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        public static string ReplaceAt(this string input, int index, string subStr){
            return input.Remove(index, subStr.Length).Insert(index, subStr);
        }

        public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

        /// <summary>Return the correct indefinite article for the string.</summary>
        public static string A_An(this string noun) => Regex.IsMatch(noun, "^([aeio]|un|ul)", RegexOptions.IgnoreCase) ? "an" : "a";
        
        // https://stackoverflow.com/a/20166/
        public static string Ord(this int number)
        {
            var str = number.ToString();
            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
                return str + "th";
            switch (number % 10)
            {
                case 1: return str + "st";
                case 2: return str + "nd";
                case 3: return str + "rd";
                default: return str + "th";
            }
        }
    }
}