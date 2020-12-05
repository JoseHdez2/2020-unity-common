using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

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
    }
}