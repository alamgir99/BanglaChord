using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileAppServer.Models
{
    public class DataProvider
    {
        BanglaChord.Models.SongDataProvider m_Prov; // data provider
        public int m_songPerPage { get; set; }
        public int m_totalPage { get; set; }
     
        public DataProvider(int songPerPage = 10, BanglaChord.Models.SongFilter filter = null)
        {
            string dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"].ToString();
            BanglaChord.Models.BanglaChordContext dbCon = new BanglaChord.Models.BanglaChordContext(dbName);
            m_Prov = new BanglaChord.Models.SongDataProvider(dbCon, filter);
            m_totalPage = (int)Math.Ceiling(m_Prov.TotalSongCount() / (double)songPerPage);
            m_songPerPage = songPerPage;
        }

        //return a list of songs
        public List<BanglaChord.Models.SongMeta> GetSongMetaList()
        {
            return m_Prov.GetSongMetaList().ToList();           
        }

        //returns a full view of a song
        public SongFull GetSong(int songId)
        {
            //create an empty object
            SongFull songFull = new SongFull();

            //get the data
            BanglaChord.Models.Song aSong = m_Prov.GetSong(songId);
            if (aSong != null)
            {
                songFull.SongId = songId; // aSong.SongId;
                songFull.Title = aSong.Title;
                songFull.Artist = aSong.Artist;
                songFull.Album = aSong.Album;
                if(aSong.Keywords != null)
                    songFull.Keywords = aSong.Keywords.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
                songFull.PublishedOn = aSong.PublishedOn;
           
                //return raw text, no need to parse, the mobile app will do that
                songFull.Text = aSong.SongText; 
           
                return songFull;
            }
            else
                return null;
        }
    }
}