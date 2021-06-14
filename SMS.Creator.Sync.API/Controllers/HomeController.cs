using RestSharp;
using SMS.Creator.Common.Constant;
using SMS.Creator.Sync.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SMS.Creator.Sync.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult PushSMS(string sms = "")
        {
            if (!string.IsNullOrEmpty(sms))
            {
                ServicePointManager.Expect100Continue = true;
                IRestResponse IRestResponse = null;
                ViewBag.Title = "Home Page";

                SMSRootDataModel rootObj = new SMSRootDataModel();
                SMSDataModel obj = new SMSDataModel()
                {
                    SMS = sms
                };
                rootObj.data = obj;
                IRestResponse = ZohoServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoAPIUrlConstant.PostSMS, Method.POST, rootObj);
                if (IRestResponse != null)
                {
                    var responseString =
                    ViewBag.Response = IRestResponse.Content + " From SMS:" + sms;
                    if (IRestResponse.StatusCode == HttpStatusCode.OK)
                    {

                    }
                }
            }
            else
            {
                ViewBag.Response = "Page is working";
            }
            return View();
        }
    }
}
