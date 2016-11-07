using BanglaChord.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileAppServer.Models
{
    public class SongShort
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
     }

    public class SongFull
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string[] Keywords { get; set; }
        public DateTime PublishedOn { get; set; }
        public string Text { get; set; }
    }
}