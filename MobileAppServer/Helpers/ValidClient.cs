using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileAppServer.Helpers
{
    public class ValidClient
    {
        //a valid iOS or Android app will send a clientid code
        //that will be verified here before responding to  a request
        public static bool isValidClient(int? clientid)
        {
            return true;
        }
    }
}