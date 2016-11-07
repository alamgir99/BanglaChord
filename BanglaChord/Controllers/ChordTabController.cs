using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BanglaChord.Models;
using BanglaChord.ViewModels;
using BanglaChord.Helpers;
using System.Web.UI;

namespace BanglaChord.Controllers
{
    public class ChordTabController : Controller
    {

        //lists songs chord/tab
        //ex. bc.com/ChordTab/
        //    bc.com/ChordTab/Page/2
        [OutputCache(Duration = int.MaxValue, VaryByParam = "page", Location = OutputCacheLocation.Server)]
        public ActionResult Index(int page = 1)
        {
            SongViewDataProvider prov = new SongViewDataProvider();
            PagerHelper pager = new PagerHelper(prov.m_totalPage, 5);

            if (page > 0 && page <= prov.m_totalPage)
                ViewBag.curPage = page;
            else
                ViewBag.curPage = 1;

            //attach the pager
            pager.SetCurrentPage(ViewBag.curPage);
            ViewBag.Pager = pager;

            ViewBag.songs = prov.GetSongs(ViewBag.curPage);
            //latest songs list
            ViewBag.LatestSongs = prov.GetLatestSongs(5); // 5 latest songs


            ViewBag.Action = "Index";
            ViewBag.PageName = "ChordTab";

            return View("ChordTabList");
        }

        //lists songs chord/tab for an artist
        //ex. bc.com/ChordTab/Artist/James
        //    bc.com/ChordTab/Artist/Page/2
        [OutputCache(Duration = int.MaxValue, VaryByParam = "artistName", Location = OutputCacheLocation.Server)]
        public ActionResult Artist(string artistName, int page = 1)
        {
            SongViewDataProvider prov = new SongViewDataProvider(10, new SongFilter { Artist = artistName });
            PagerHelper pager = new PagerHelper(prov.m_totalPage, 5);

            ViewBag.totPage = prov.m_totalPage;
            if (page > 0 && page <= ViewBag.totPage)
                ViewBag.curPage = page;
            else
                ViewBag.curPage = 1;

            //attach the pager
            pager.SetCurrentPage(ViewBag.curPage);
            ViewBag.Pager = pager;

            ViewBag.Artist = artistName;
            ViewBag.songs = prov.GetSongs(ViewBag.curPage);
            ViewBag.Action = "Artist";

            //latest songs list
            ViewBag.LatestSongs = prov.GetLatestSongs(5); // 5 latest songs

            return View("ChordTabList");
        }

        //lists songs chord/tab for an album
        //ex. bc.com/ChordTab/Album/Kosto
        //    bc.com/ChordTab/Album/Kosto/Page/2
        [OutputCache(Duration = int.MaxValue, VaryByParam = "albumName", Location = OutputCacheLocation.Server)]
        public ActionResult Album(string albumName, int page = 1)
        {
            SongViewDataProvider prov = new SongViewDataProvider(10, new SongFilter { Album = albumName });
            PagerHelper pager = new PagerHelper(prov.m_totalPage, 5);

            ViewBag.totPage = prov.m_totalPage;
            if (page > 0 && page <= ViewBag.totPage)
                ViewBag.curPage = page;
            else
                ViewBag.curPage = 1;

            //attach the pager
            pager.SetCurrentPage(ViewBag.curPage);
            ViewBag.Pager = pager;


            ViewBag.Album = albumName;
            ViewBag.songs = prov.GetSongs(ViewBag.curPage);
            ViewBag.Action = "Album";

            //latest songs list
            ViewBag.LatestSongs = prov.GetLatestSongs(5); // 5 latest songs

            return View("ChordTabList");
        }

        //show the full view of a song
        //ex. bc.com/ChordTab/Detail/5/times-in-a-bottle
        //     bc.com/ChordTab/Detail/4
        //     bc.com/ChordTab/Detail/10/Transpose/2

        [OutputCache(Duration = int.MaxValue, VaryByParam = "SongId;tkey", Location = OutputCacheLocation.Server)]
        public ActionResult Detail(int SongId, int tkey = 0, string seoTitle="") 
        {
            SongViewDataProvider prov = new SongViewDataProvider();
            ViewBag.Song = prov.GetSong(SongId, tkey);

            //latest songs list
            ViewBag.LatestSongs = prov.GetLatestSongs(5); // 5 latest songs

            return View("ChordTabDetail");
        }
    }
}
