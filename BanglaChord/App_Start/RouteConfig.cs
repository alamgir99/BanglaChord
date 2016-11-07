using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BanglaChord.Helpers;

namespace BanglaChord
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* begin - related to article aka post */
            
            //show detail view of an article
            routes.MapRoute(
                name: "ArticleDetail",
                url: "Article/Detail/{postId}/{SEOTitle}",
                defaults: new { controller = "Article", action = "Detail", postId = UrlParameter.Optional,  SEOTitle = UrlParameter.Optional }
            );
            
            
            //show list of articles for a tag
            routes.MapRoute(
                name: "ArticleByTag",
                url: "Article/Tag/{tagWord}/Page/{Page}",
                defaults: new { controller = "Article", action = "Tag", tagWord = UrlParameter.Optional, Page = UrlParameter.Optional }
            );
            
            //show list of all articles
            routes.MapRoute(
                name: "ArticleList",
                url: "Article/Page/{Page}",
                defaults: new { controller = "Article", action = "Index", Page = UrlParameter.Optional }
            );
             
            /* end - related to article aka posts */

            /* begin - related to song aka ChordTab */
            
            //show detail view of a song
            routes.MapRoute(
                name: "ChordTabDetail",
                url: "ChordTab/Detail/{songId}/tkey/{tkey}/{SEOTitle}",
                defaults: new { controller = "ChordTab", action = "Detail", songId = UrlParameter.Optional, tkey = UrlParameter.Optional, SEOTitle = UrlParameter.Optional }
            );
            
            //list of songs by artist
            routes.MapRoute(
                  name: "ChordTabListArtist",
                  url: "ChordTab/Artist/{ArtistName}/Page/{Page}",
                defaults: new { controller = "ChordTab", action = "Artist", ArtistName = UrlParameter.Optional, Page = UrlParameter.Optional }
            );
            //list of songs by album
            routes.MapRoute(
                  name: "ChordTabListAlbum",
                  url: "ChordTab/Album/{AlbumName}/Page/{Page}",
                defaults: new { controller = "ChordTab", action = "Album", AlbumName = UrlParameter.Optional, Page = UrlParameter.Optional, }
            ); 
            //show list of songs
            routes.MapRoute(
                name: "ChordTabList",
                url: "ChordTab/Page/{Page}",
                defaults: new { controller = "ChordTab", action = "Index", Page = UrlParameter.Optional }
            );
            
            /* end - related to song aka ChordTab */
            
            /* AboutSite  and ContactUs */
            routes.MapRoute(
                name: "AboutSite",
                url: "AboutSite",
                defaults: new { controller = "AboutContactUs", action = "AboutSite"}
            );

            routes.MapRoute(
                name: "ContactUs",
                url: "ContactUs",
                defaults: new { controller = "AboutContactUs", action = "ContactUs" }
            );

            //not done yet
            routes.MapRoute(
               name: "NotDoneYet",
               url: "App",
               defaults: new { controller = "NotDoneYet", action = "App" }
           );
            
            //remap the url from old php-based site
               routes.Add(new Route("viewtopic.php",
                new CustomPHPRouteHandler()));
            
               routes.Add(new Route("viewsong.php",
                   new CustomPHPRouteHandler()));
            

            //LegacyUrlRoute
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            ); 
        }
    }
}