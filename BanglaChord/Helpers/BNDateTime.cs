using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanglaChord.Helpers
{
    public class BNDateTime
    {
        static string[] Months = { "জানুয়ারি", "ফেব্রুয়ারি", "মার্চ", "এপ্রিল", "মে", "জুন", "জুলাই", "অগাস্ট", "সেপ্টেম্বর", "অক্টোবর", "নভেম্বর", "ডিসেম্বর" };
        static string[] Digits = { "০", "১", "২", "৩", "৪", "৫", "৬", "৭", "৮", "৯" };

        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }



        public BNDateTime(DateTime dt)
        {
            Month = Months[dt.Month - 1];
            Day = EnToBNNumber(dt.Day.ToString());
            Year = EnToBNNumber(dt.Year.ToString());
        }

        public static string EnToBNNumber(string en)
        {
            string bn = "";
            string bc;

            for (int i = 0; i < en.Length; i++)
            {
                bc = Digits[en[i] - 48];
                bn += bc;
            }
            return bn;
        }

        override public string  ToString()
        {
            return Month + " "+Day + ", " + Year;
        }
    }
}