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
    public class ArticleController : Controller
    {

        //main home page - lists news posts
        // ex: bc.com/Article
        //     bc.com/Article/page/3
        [OutputCache(Duration = int.MaxValue, VaryByParam = "page", Location = OutputCacheLocation.Server)]
        public ActionResult Index(int page=1)
        {
            PostViewDataProvider prov = new PostViewDataProvider();
            PagerHelper pager = new PagerHelper(prov.m_totalPage, 5);

            ViewBag.totPage = prov.m_totalPage;
            if (page > 0 && page <= ViewBag.totPage)
                ViewBag.curPage = page;
            else
                ViewBag.curPage = 1;

            //attach the pager
            pager.SetCurrentPage(ViewBag.curPage);
            ViewBag.Pager = pager;

            
            ViewBag.posts = prov.GetPosts(ViewBag.curPage);
           //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            ViewBag.PageName = "Article";

            return View("ArticleList");
        }

        //show the detail view of a post
        // ex: bc.com/Article/Detail/2/this-is-good
        //     bc.com/Article/Detail/5
        [OutputCache(Duration = int.MaxValue, VaryByParam = "postId", Location = OutputCacheLocation.Server)]
        public ActionResult Detail(int postId, string seoTitle="")
        {
            PostViewDataProvider prov = new PostViewDataProvider();

            
            ViewBag.Post = prov.GetPost(postId);

            //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            return View("ArticleDetail");
        }

        //show list of article for a tagword
        //ex bc.com/Article/Tag/
        [OutputCache(Duration = int.MaxValue, VaryByParam = "tagWord", Location = OutputCacheLocation.Server)]
         public ActionResult Tag(string tagWord, int page = 1)
        {
            PostViewDataProvider prov = new PostViewDataProvider(5, tagWord);
            PagerHelper pager = new PagerHelper(prov.m_totalPage, 5);

            ViewBag.totPage = prov.m_totalPage;
            if (page > 0 && page <= ViewBag.totPage)
                ViewBag.curPage = page;
            else
                ViewBag.curPage = 1;

            //attach the pager
            pager.SetCurrentPage(ViewBag.curPage);
            ViewBag.Pager = pager;


            ViewBag.posts = prov.GetPosts(ViewBag.curPage);
            //latest songs list
            SongViewDataProvider sprov = new SongViewDataProvider();
            ViewBag.LatestSongs = sprov.GetLatestSongs(5); // 5 latest songs

            return View("ArticleList");
        }
    }
}
