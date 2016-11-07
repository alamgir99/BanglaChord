using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanglaChord.Helpers
{
    public static class Extensions
    {
        //replace a  character in a string at given location
        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }

        //slice an array
        public static T[] Slice<T>(this T[] arr, int indexFrom, int indexTo)
        {
            if (indexFrom > indexTo)
            {
                throw new ArgumentOutOfRangeException("indexFrom is bigger than indexTo!");
            }

            if (indexFrom < 0 || indexTo < 0)
            {
                throw new ArgumentOutOfRangeException("Negative index!");
            }

            int length = indexTo - indexFrom;
            T[] result = new T[length];
            Array.Copy(arr, indexFrom, result, 0, length);

            return result;
        }
    }
}