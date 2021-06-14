using Newtonsoft.Json;
using RestSharp;
using SMS.Creator.Common.Constant;
using SMS.Creator.Common.Helper;
using SMS.Creator.Sync.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SMS.Creator.Sync.API.Controllers
{
    public class SMSController : ApiController
    {
       /// <summary>
       /// Method to POST SMS
       /// </summary>
       /// <param name="SMS"></param>
       /// <returns></returns>
        // POST api/values
        [HttpPost]
        public string Post(string SMS)
        {
            ServicePointManager.Expect100Continue = true;
            IRestResponse IRestResponse = null;
            try
            {
                SMSRootDataModel rootObj = new SMSRootDataModel();
                SMSDataModel obj = new SMSDataModel()
                {
                    SMS = SMS
                };
                rootObj.data = obj;
                 IRestResponse =ZohoServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoAPIUrlConstant.PostSMS,Method.POST, rootObj);
                if (IRestResponse != null)
                {
                       var responseString = IRestResponse.Content;
                      if (IRestResponse.StatusCode == HttpStatusCode.OK)
                    {
                    }
                  }
             }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SMS", "Post", ex);

            }

            return IRestResponse.Content;
        }
  
      
    }
}
