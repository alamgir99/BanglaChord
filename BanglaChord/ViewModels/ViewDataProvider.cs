using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BanglaChord.Models;
using BanglaChord.Helpers;
using System.Text.RegularExpressions;

namespace BanglaChord.ViewModels
{
    public class PostViewDataProvider
    {
        ArticleDataProvider m_Prov; // data provider
        public int m_postPerPage {get; set;}
        public int m_totalPage {get; set;}
        public IDictionary<int, string> m_Users;

        public PostViewDataProvider(int postPerPage = 5, string tagfilter="")
        {
            string dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"].ToString();
            BanglaChordContext dbCon = new BanglaChordContext(dbName);
            m_Prov = new ArticleDataProvider(dbCon, tagfilter);

            m_postPerPage = postPerPage;
            m_totalPage = (int)Math.Ceiling(m_Prov.TotalPostCount() / (double)postPerPage);

            m_Users = new Dictionary<int, string>();
            foreach (var user in m_Prov.GetUsers())
            {
                m_Users[user.UserId] = user.Alias;
            }
        }

        //return a list of posts
        public IEnumerable<PostShort> GetPosts(int page = 1)
        {
            IEnumerable<Post> selPosts = m_Prov.GetPosts(page, m_postPerPage);

            if (selPosts == null) yield return null;

            foreach (Post aPost in selPosts)
            {
                //create an empty object
                PostShort postShort = new PostShort();

                //copy the data
                postShort.PostId = aPost.PostId;
                postShort.Title = aPost.Title;
                //deal with SEO friendly url here
                postShort.SEOTitle = aPost.Title.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen
                //multple - to single one
                postShort.SEOTitle = Regex.Replace(postShort.SEOTitle, "-+", "-");
                postShort.TagWords = aPost.TagWords.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                postShort.UserAlias = m_Users[aPost.UserId];
                postShort.PublishedOn = new BNDateTime(aPost.PublishedOn);

                int teaserEnd = aPost.PostText.IndexOf("<tbreak>");
                if (teaserEnd > -1) // found a break
                {
                    postShort.TeaserText = aPost.PostText.Substring(0, teaserEnd);
                }
                else
                {
                    postShort.TeaserText = aPost.PostText.Substring(0, aPost.TeaserLength);
                }
                postShort.TeaserText = postShort.TeaserText.Replace("<p>", "").Replace("</p>", "").Replace("<br />", "");
                
                yield return postShort;               
            }
        }

        
        //returns a full view of the post
        public PostFull GetPost(int postId)
        {
            //create an empty object
            PostFull postFull = new PostFull();

            //get the data
            Post aPost = m_Prov.GetPost(postId);
            postFull.PostId = aPost.PostId;
            postFull.Title = aPost.Title;
            postFull.TagWords = aPost.TagWords.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            postFull.UserAlias = m_Users[aPost.UserId];
            postFull.PublishedOn = new BNDateTime(aPost.PublishedOn);
            postFull.EditedOn = new BNDateTime(aPost.EditedOn);
            postFull.Text = aPost.PostText.Replace("<tbreak>",""); // remove any possible teaser break code
            postFull.CommentLink = aPost.CommentLink;

            return postFull;
        }

    }


    public class SongViewDataProvider
    {
        SongDataProvider m_Prov; // data provider
        public int m_songPerPage { get; set; }
        public int m_totalPage { get; set; }
        public IDictionary<int, string> m_Users;

        public SongViewDataProvider(int songPerPage = 10, SongFilter filter = null)
        {
            string dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"].ToString();
            BanglaChordContext dbCon = new BanglaChordContext(dbName);
            m_Prov = new SongDataProvider(dbCon, filter);
            m_totalPage = (int)Math.Ceiling(m_Prov.TotalSongCount() / (double)songPerPage);
            m_songPerPage = songPerPage;
            
            m_Users = new Dictionary<int, string>();
            foreach (var user in m_Prov.GetUsers())
            {
                m_Users[user.UserId] = user.Alias;
            }
        }

        //return a list of songs
        public IEnumerable<SongShort> GetSongs(int page = 1)
        {
            IEnumerable<Song> selSongs = m_Prov.GetSongs(page, m_songPerPage);

            if (selSongs == null) yield return null;

            foreach (Song aSong in selSongs)
            {
                //create an empty object
                SongShort songShort = new SongShort();

                //copy the data
                songShort.SongId = aSong.SongId;
                songShort.Title = aSong.Title;
                //deal with SEO friendly url here
                songShort.SEOTitle = aSong.Title.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen
                //multple - to single one
                songShort.SEOTitle = Regex.Replace(songShort.SEOTitle, "-+", "-");
                songShort.Artist = aSong.Artist;
                songShort.Album = aSong.Album;

                songShort.PublishedOn = new BNDateTime(aSong.PublishedOn);
                
                yield return songShort;
            }
        }

        // return a list of latest songs
        public IQueryable<SelectedSong> GetLatestSongs(int count)
        {
            return m_Prov.GetLatestSongs(count);
        }

        //returns a full view of a song
        public SongFull GetSong(int songId, int tKey=0)
        {
            //create an empty object
            SongFull songFull = new SongFull();

            //get the data
            Song aSong = m_Prov.GetSong(songId);
            if (aSong != null)
            {
                songFull.SongId = songId; // aSong.SongId;
                songFull.Title = aSong.Title;
                if (aSong.Artist != null)
                    songFull.Artist = aSong.Artist.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen;
                if (aSong.TuneBy != null)
                    songFull.TuneBy = aSong.TuneBy.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen;
                if (aSong.LyricBy != null)
                    songFull.LyricBy = aSong.LyricBy.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen;
                if (aSong.Album != null)
                    songFull.Album = aSong.Album.Replace(' ', '-').Replace(':', '-').Replace('/', '-').Replace(',', '-'); // replace <space>, colon, slash and comma with a hyphen;
                if(aSong.Keywords != null)
                    songFull.Keywords = aSong.Keywords.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                songFull.PublishedOn = new BNDateTime(aSong.PublishedOn);
                songFull.EditedOn = new BNDateTime(aSong.EditedOn);
                songFull.UserAlias = m_Users[aSong.UserId];
                songFull.Transpose = tKey; // transpose a song if needed, use tKey

                ChordTabParser ctParser = new ChordTabParser(aSong.SongText, tKey);
                //songFull.Text = ctParser.Parse("AponaLohit", "500px", "800px");
                songFull.Text = ctParser.Parse("Hind Siliguri");
                songFull.CommentLink = aSong.CommentLink;

                return songFull;
            }
            else
                return null;
        }

    }
}