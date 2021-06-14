using Newtonsoft.Json;
using RestSharp;
using SMS.Creator.Common.Helper;

using SMS.Creator.Sync.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace SMS.Creator.Common.Constant
{
    public class ZohoServiceCall
    {      ///// <summary>
        ///// Method to call zoho creator API to add content
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="methodtype"></param>
        ///// <param name="content">Response</param>
        ///// <returns></returns>


        public static IRestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, Method methodType, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            try
            {
                #region  Code to generate the auth tocken by refresh token                
                AouthToken aouthObj = Rest_InvokeZohoInvoiceAuthToken();
                if (aouthObj != null)
                    aouthToken = aouthObj.access_token;
                #endregion

                if (!string.IsNullOrEmpty(aouthToken))
                {
                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.Method = methodType;
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader(HeaderConstant.Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);
                    if (content != null)
                    {
                        jsonString = JsonConvert.SerializeObject(content);
                        request.AddJsonBody(jsonString);
                    }
                    getResponse = client.Execute(request);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeZohoInvoiceServiceForPlainText", ex);

            }
            return getResponse;

        }

        /// <summary>
        // / Method to generate auth token by refresh token
        // / </summary>
        // / <returns>AouthToken</returns>
        public static AouthToken Rest_InvokeZohoInvoiceAuthToken()
        {
            HttpWebResponse httpWebResp = null;
            AouthToken aouthObj = null;
            XMLHelper XMLHelperOBJ = new XMLHelper();
            CurrentAuthtokenDataModel CurrentDataModel = new CurrentAuthtokenDataModel();
            string xml = HttpContext.Current.Server.MapPath(@"~\App_Data\CreatorAuthToken.xml");
            try
            {
                ServicePointManager.Expect100Continue = true;
                System.Net.ServicePointManager.SecurityProtocol =
           SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                List<CurrentAuthtokenDataModel> currentToken = XMLHelperOBJ.LoadConfiguration();
                if (currentToken != null)
                {
                    if (CheckTokenValid(currentToken))
                    {
                        aouthObj = new AouthToken();
                        aouthObj.access_token = currentToken[0].CurrentAuthToken.ToString();
                    }
                    else
                    {
                        string url = ZohoAPIUrlConstant.GenerateAccessTokenURL;
                        url = url.Replace("[refresh-token]", ConfigurationManager.AppSettings["refresh-token"].ToString());
                        url = url.Replace("[client-id]", ConfigurationManager.AppSettings["client-id"].ToString());
                        url = url.Replace("[client-secret]", ConfigurationManager.AppSettings["client-secret"].ToString());
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = MethodTypeConstant.POST;

                        httpWebResp = (HttpWebResponse)request.GetResponse();

                        if (httpWebResp != null)
                        {
                            Stream stream = httpWebResp.GetResponseStream();
                            if (stream != null)
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string result = reader.ReadToEnd();
                                    aouthObj = JsonConvert.DeserializeObject<AouthToken>(result);
                                }
                            }
                        }

                        #region Code to update the token in 
                        foreach (CurrentAuthtokenDataModel courseDCObj in currentToken)
                        {
                            if (courseDCObj.CreatedDate != null)
                            {
                                bool update = XMLHelperOBJ.UpdateConfig("CreatedDate", DateTime.Now.ToString(), xml);
                            }
                            if (currentToken[0].CurrentAuthToken != null)
                            {
                                bool update = XMLHelperOBJ.UpdateConfig("CurrentAuthToken", aouthObj.access_token, xml);

                            }

                        }
                       
                        #endregion
                    }
                }
                else
                {
                    //Code to add auth token
                    string url = ZohoAPIUrlConstant.GenerateAccessTokenURL;
                    url = url.Replace("[refresh-token]", ConfigurationManager.AppSettings["refresh-token"].ToString());
                    url = url.Replace("[client-id]", ConfigurationManager.AppSettings["client-id"].ToString());
                    url = url.Replace("[client-secret]", ConfigurationManager.AppSettings["client-secret"].ToString());
                    //url = url.Replace("[redirect-uri]", ConfigurationManager.AppSettings["redirect-uri"].ToString());
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = MethodTypeConstant.POST;

                    httpWebResp = (HttpWebResponse)request.GetResponse();

                    if (httpWebResp != null)
                    {
                        Stream stream = httpWebResp.GetResponseStream();
                        if (stream != null)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                string result = reader.ReadToEnd();
                                aouthObj = JsonConvert.DeserializeObject<AouthToken>(result);
                            }
                        }
                    }

                    #region Code to update the token in DB
                    foreach (CurrentAuthtokenDataModel courseDCObj in currentToken)
                    {
                        if (courseDCObj.CreatedDate != null)
                        {
                            bool update = XMLHelperOBJ.UpdateConfig("CreatedDate", DateTime.Now.ToString(), xml);
                        }
                        if (currentToken[0].CurrentAuthToken != null)
                        {
                            bool update = XMLHelperOBJ.UpdateConfig("CurrentAuthToken", aouthObj.access_token, xml);

                        }

                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoServiceCalls", "Rest_InvokeZohoInvoiceAuthToken", ex);

            }
            return aouthObj;
        }

        /// <summary>
        /// Check access token validity
        /// </summary>
        /// <param name="currentToken"></param>
        /// <returns>bool</returns>
        public static bool CheckTokenValid(List<CurrentAuthtokenDataModel> currentToken)
        {

            bool flag = false;
            try
            {
                if (currentToken != null && currentToken[0].CreatedDate != null)
                {
                    TimeSpan ts = DateTime.Now - Convert.ToDateTime(currentToken[0].CreatedDate);
                    if (ts.TotalMinutes > 30)
                        flag = false;
                    else
                        flag = true;
                }
                else
                {
                    //generate new 
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// Method For AouthToken
        /// </summary>
        public class AouthToken
        {
            public string access_token { get; set; }
            public string api_domain { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
        }

    }
}