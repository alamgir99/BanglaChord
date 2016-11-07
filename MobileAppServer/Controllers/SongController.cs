using MobileAppServer.Helpers;
using MobileAppServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace MobileAppServer.Controllers
{
    public class SongController : ApiController
    {
        //URL:  webapi.banglachord.com/api/Song?authCode=1&songId=xx
        //return a full view of a song
        public string Get(int? authCode, int songId)
        {
            //verify the authencity of the requesting app
            if (ValidClient.isValidClient(authCode) != true)
                return null;

            DataProvider prov = new DataProvider();
            SongFull song = prov.GetSong(songId);

            if (song != null)
            {

                JavaScriptSerializer json = new JavaScriptSerializer();
                string result = json.Serialize(song);
                return result;
            }
            else
                return "";

        }
    }
}
