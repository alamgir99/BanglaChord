using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BanglaChord.Helpers
{
    //http://www.hanselman.com/blog/BackToBasicsDynamicImageGenerationASPNETControllersRoutingIHttpHandlersAndRunAllManagedModulesForAllRequests.aspx

    public class CustomPHPRouteHandler : IRouteHandler
    {
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new CustomPHPHandler(requestContext);
        }
    }

    public class CustomPHPHandler : IHttpHandler
    {
        public bool IsReusable { get { return false; } }
        protected RequestContext RequestContext { get; set; }

        public CustomPHPHandler() : base() { }

        public CustomPHPHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var res = context.Response;

            var postId = req.QueryString.Count != 0 ? req.QueryString[0] : "";
            var newUrl = "";
            if (req.RawUrl.IndexOf("topic") > 1)
            {
                newUrl = "Article/Detail/" + postId;
            }
            else if (req.RawUrl.IndexOf("song") > 1)
            {
                newUrl = "ChordTab/Detail/" + postId;
            }
            
            
            const string status = "301 Moved Permanently";

            res.Status = status;
            res.RedirectLocation = newUrl;
            res.End();
        }
    }
}