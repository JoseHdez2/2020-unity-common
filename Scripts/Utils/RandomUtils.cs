using System;

namespace ExtensionMethods
{
    public static class RandomUtils
    {
        static Random _R = new Random ();
        public static T RandomEnumValue<T> (){
            var v = Enum.GetValues (typeof (T));
            return (T) v.GetValue (_R.Next(v.Length));
        }

        public static T RandomEnumValue<T> (System.Random rand){
            var v = Enum.GetValues (typeof (T));
            return (T) v.GetValue (rand.Next(v.Length));
        }
    }
}