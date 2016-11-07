using BanglaChord.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanglaChord.ViewModels
{
    public class PostShort
    {
        public int PostId { get; set; } // need for link creation
        public string Title { get; set; }
        public string SEOTitle { get; set; }
        public string[] TagWords { get; set; }
        public string UserAlias { get; set; }
        public BNDateTime PublishedOn { get; set; }
        public string TeaserText { get; set; }
    }

    public class PostFull
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string[] TagWords { get; set; }
        public string UserAlias { get; set; }
        public BNDateTime EditedOn { get; set; }
        public BNDateTime PublishedOn { get; set; }
        public string Text { get; set; }
        public string CommentLink { get; set; }
    }

    public class SongShort
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string SEOTitle { get; set; }
        public string Artist{ get; set; }
        public string Album { get; set; }
        public BNDateTime PublishedOn { get; set; }
     }

    public class SongFull
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string TuneBy { get; set; }
        public string LyricBy { get; set; }
        public string Album { get; set; }
        public string[] Keywords{get;set;}
        public BNDateTime EditedOn { get; set; }
        public BNDateTime PublishedOn { get; set; }
        public string UserAlias { get; set; }
        public int Transpose { get; set; }
        public string Text { get; set; }
        public string CommentLink { get; set; }
    }
}