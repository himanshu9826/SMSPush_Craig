using SMS.Creator.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SMS.Creator.Sync.API.Controllers
{
    public class TestController : ApiController
    {
        /// <summary>
        /// Mehtod to write SMS in Log
        /// </summary>
        /// <param name="SMS"></param>
        /// <returns></returns>

        [HttpPost]
        public string Post(string SMS)
        {
            string data = SMS;
            try
            {
                LibLogging.WriteSMSToLog("SMS", "Post",data);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SMS", "Post", ex);

            }

            return SMS;
        }
    }
}
