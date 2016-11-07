using BanglaChord.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanglaChord.Controllers
{
    public class AboutContactUsController : Controller
    {
        //
        // GET: /AboutSite/
        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult AboutSite()
        {
            ViewBag.PageName = "AboutSite";

            //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            return View("Index");
        }

        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult ContactUs()
        {
            ViewBag.PageName = "ContactUs";

            //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            return View("Index");
        }

    }
}
