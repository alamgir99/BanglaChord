using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using MobileAppServer.Models;
using BanglaChord.ViewModels;
using System.Web.Script.Serialization;
using MobileAppServer.Helpers;

namespace MobileAppServer.Controllersxamari
{
    public class SongListController : ApiController
    {

        //URL:  webapi.banglachord.com/api/SongList?authCode=1&page=xx
        //public MobileAppServer.Models.SongShort[] Get(int page=1)
        public string Get(int? authCode)
        {
            //verify the authencity of the requesting app
            if (ValidClient.isValidClient(authCode) != true)
                return null;

            DataProvider prov = new DataProvider();

            List<BanglaChord.Models.SongMeta> songs = prov.GetSongMetaList();

            //MobileAppServer.Models.SongShort[] songArray = new Models.SongShort[songs.Count()+1];

            JavaScriptSerializer json = new JavaScriptSerializer();
            string result = json.Serialize(songs);
            return result;
        }

    }
}
