using UnityEngine;
using System.Collections;

public enum ECase
{
    NONE, UPPERCASE, LOWERCASE, CAPITALIZE
}

namespace ExtensionMethods
{
    public static class CaseExtensions
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
    }
}