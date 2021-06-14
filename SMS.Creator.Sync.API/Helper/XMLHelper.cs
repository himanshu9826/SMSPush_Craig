
using SMS.Creator.Sync.API.Helper;
using SMS.Creator.Sync.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;
using static SMS.Creator.Sync.API.Helper.GenericConversionHelper;

namespace SMS.Creator.Common.Helper
{
    public class XMLHelper
    {

        public List<CurrentAuthtokenDataModel> LoadConfiguration()
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            List<CurrentAuthtokenDataModel> currentAuth = null;
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(HttpContext.Current.Server.MapPath(@"~\App_Data\CreatorAuthToken.xml"));
                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(new System.IO.StringReader(document.InnerXml));
                reader.Read();
                ds.ReadXml(reader, System.Data.XmlReadMode.Auto);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];                  
                }
                currentAuth = GenericConversionHelper.DataTableToList<CurrentAuthtokenDataModel>(dt);

                ds.WriteXml(HttpContext.Current.Server.MapPath(@"~\App_Data\CreatorAuthToken.xml"));

              }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("XMLHelper", "LoadConfiguration", ex);

            }

            return currentAuth;
        }

        public bool UpdateConfig(string columnName, string value,string xmlpath)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            bool flag = false;
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(xmlpath);
                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(new System.IO.StringReader(document.InnerXml));
                reader.Read();
                ds.ReadXml(reader, System.Data.XmlReadMode.Auto);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    dt.Rows[0][columnName] = value;
                }
                ds.WriteXml(xmlpath);
                flag = true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("XMLHelper", "UpdateConfig", ex);

                flag = false;
            }
            return flag;
        }
    }
}