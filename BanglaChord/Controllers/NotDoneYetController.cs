using BanglaChord.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace BanglaChord.Controllers
{
    public class NotDoneYetController : Controller
    {

        [OutputCache(Duration = int.MaxValue, VaryByParam = "none")]
        public ActionResult App()
        {
            ViewBag.Message = "The apps are still in the process of development and not released in public yet. Please check back later.";
            
            ViewBag.PageName = "App";

            //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            return View("BCApp");

        }
    }
}
