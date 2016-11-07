using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
namespace BanglaChord.Models
{
    //models the Article
    public class ArticleDataProvider
    {
        //database context
        BanglaChordContext m_dBContext;
        string m_TagFilter;
        
        public ArticleDataProvider(BanglaChordContext con, string tag="")
        {
            m_dBContext = con;
            m_TagFilter = tag;

        }
        
        //return total count of posts
        public int TotalPostCount() {
            int count = 0;
            if (m_TagFilter != null)
            {
                if (m_TagFilter != "") {
                    IEnumerable<Post> posts =  m_dBContext.Posts; //.Where(p => p.TagWords.Contains(m_TagFilter)); // apply tag metacode
                    foreach (Post p in posts) {
                        string tagw = p.TagWords;
                        bool found = tagw.Contains(m_TagFilter);
                        if (found == true)
                            count++;
                    } 
                    //count = posts.Count();
                    return count; 
                }
 
            }
            return m_dBContext.Posts.Count();
        }
        //return a single post
        public Post GetPost(int postId)
        {
            return m_dBContext.Posts.FirstOrDefault(p => p.PostId == postId );
        }

        //return a chosen set of posts
        public IEnumerable<Post> GetPosts(int page = 1, int pageSize = 1)
        {
            int pageCount = m_dBContext.Posts.Count(); // consider each post a page
            if (pageSize > 1)
                pageCount = (int)Math.Ceiling(pageCount / (1.0 * pageSize));

            if (page < 1 || page > pageCount)
                return null;

            IEnumerable<Post> selPosts = m_dBContext.Posts.Where(p => p.IsPublished == true).OrderByDescending(p => p.PublishedOn)
                                                            .Skip((page - 1) * pageSize)
                                                            .Take(pageSize).AsEnumerable<Post>();

            if (m_TagFilter != null)
            {
                if (m_TagFilter != "")
                    selPosts = selPosts.Where(p => p.TagWords.Contains(m_TagFilter));
                
            }

            return selPosts;
        }
          
 
        //add a post
        public bool AddPost(Post aPost)
        {
            m_dBContext.Posts.Add(aPost);
            return true;
        }

        //delete a post
        public bool DeletePost(int pId)
        {
            //locate the post
            var item = m_dBContext.Posts.SingleOrDefault(x => x.PostId == pId);
            if (item != null)
            {
                m_dBContext.Posts.Remove(item);
                m_dBContext.SaveChanges();
            }
            return true;
        }

        //edit a post
        public bool EditPost(Post aPost)
        {
            m_dBContext.Entry(aPost).State = System.Data.Entity.EntityState.Modified;
            m_dBContext.SaveChanges();
         
            return true;
        }

        //users
        //return list of users
        public IEnumerable<User> GetUsers()
        {
            return m_dBContext.Users.OrderBy(u => u.UserId);
        }

    }

    public class SongDataProvider 
    {
        //database context
        BanglaChordContext m_dBContext;
        SongFilter m_Filter;
        
        public SongDataProvider(BanglaChordContext con, SongFilter filter)
        {
            m_dBContext = con;
            m_Filter = filter;
        }
        
        //functions related to Song
        //return total count of posts
        public int TotalSongCount()
        {
            int count = 0;
            if (m_Filter != null)
            {
                if (m_Filter.Keyword != "")
                { // apply metaphone
                    foreach (Song s in m_dBContext.Songs)
                    {
                        if (s.Keywords != null)
                            if (s.Keywords.Contains(m_Filter.Keyword))
                                count++;
                    }
                }
                else if (m_Filter.Artist != "")
                {
                    foreach (Song s in m_dBContext.Songs)
                    {
                        if (s.Artist.Contains(m_Filter.Artist))
                            count++;
                    }
                }
                else if (m_Filter.Album != "")
                {
                    foreach (Song s in m_dBContext.Songs)
                    {
                        if(s.Album != null)
                            if (s.Album.Contains(m_Filter.Album))
                                count++;
                    }
                }
            }
            else count = m_dBContext.Songs.Count();

            return count;
        }

        //return a single song
        public Song GetSong(int songId)
        {
            return m_dBContext.Songs.FirstOrDefault(s => s.SongId == songId);
        }

        //return a chosen set of songs
        public IEnumerable<Song> GetSongs(int page = 1, int pageSize = 1)
        {
            int pageCount = m_dBContext.Songs.Count(); // consider each song a page
            if (pageSize > 1)
                pageCount = (int)Math.Ceiling(pageCount / (1.0 * pageSize));

            if (page < 1 || page > pageCount)
                return null;
            
            IEnumerable<Song> selSongs;
            if (m_Filter != null)
            {
                selSongs = m_dBContext.Songs.OrderByDescending(s => s.PublishedOn).Where( s => (s.IsPublished == true && (s.Artist == m_Filter.Artist || s.Album == m_Filter.Album)))
                                                          .Skip((page - 1) * pageSize).Take(pageSize);
            }
            else {
                selSongs = m_dBContext.Songs.OrderByDescending(s => s.PublishedOn).Where(s => s.IsPublished == true).Skip((page - 1) * pageSize).Take(pageSize);
            }


            if (m_Filter != null)
            {
             if (m_Filter.Artist != "")
                {
                    return selSongs.Where(s => s.Artist == m_Filter.Artist);
                }
                else if (m_Filter.Album != "")
                {
                    return selSongs.Where(s => s.Album == m_Filter.Album);
                }
            }
           
            return selSongs;
        }

        //return a brief list of all songs
        public IEnumerable<SongMeta> GetSongMetaList()
        {
            var selSongs = from s in m_dBContext.Songs
                          where s.IsPublished == true
                          orderby s.SongId
                           select new SongMeta { SongId = s.SongId, EditedOn = s.EditedOn };

                     

            return selSongs;
        }


        //return a chosen set of latest songs
        public IQueryable<SelectedSong> GetLatestSongs(int count)
        {
            int songCount = m_dBContext.Songs.Count(); // consider each song a page
            
            if (count < 1 || count > songCount)
                return null;
            var selSongs = from s in m_dBContext.Songs orderby s.PublishedOn descending select new SelectedSong { SongId= s.SongId, Title = s.Title };
            
            return selSongs.Take(count);
        }

        //add a song
        public bool AddSong(Song aSong)
        {
            m_dBContext.Songs.Add(aSong);
            return true;
        }

        //delete a song
        public bool DeleteSong(int sId)
        {
            //locate the song
            var item = m_dBContext.Songs.SingleOrDefault(x => x.SongId == sId);
            if (item != null)
            {
                m_dBContext.Songs.Remove(item);
                m_dBContext.SaveChanges();
            }
            return true;
        }

        //edit a song
        public bool EditSong(Song aSong)
        {
            /*
            string dbName = System.Configuration.ConfigurationManager.AppSettings["dbName"].ToString();
            using (BanglaChordContext newCon = new BanglaChordContext(dbName))
            {
                newCon.Entry(aSong).State = System.Data.Entity.EntityState.Modified;
                newCon.SaveChanges();
            } */

            m_dBContext.Entry(aSong).State = System.Data.Entity.EntityState.Modified;
            m_dBContext.SaveChanges();
            return true;
        }


        //users
        //return list of users
        public IEnumerable<User> GetUsers()
        {
            return m_dBContext.Users.OrderBy(u => u.UserId);
        }

    }
}