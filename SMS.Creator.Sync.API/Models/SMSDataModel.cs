using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Creator.Sync.API.Models
{
    public class SMSDataModel
    {
        public object SMS_subject { get; set; }
        public object Status_of_Transaction { get; set; }
        public object Petrol_Service_Provider { get; set; }
        public object Current_Odometer { get; set; }
        public object SMS_Rand_Value { get; set; }
        public object SMS_Operation_Date_Time { get; set; }
        public object Assigned_Driver { get; set; }
        public object SMS_Date_Received_by_App { get; set; }
        public object Type { get; set; }
        public object Previous_Odo_Reading { get; set; }
        public string SMS { get; set; }
        public object Added_User { get; set; }
        public object SMS_Registration_Number { get; set; }
        public object Manual_Transaction { get; set; }
    }
    public class SMSRootDataModel
    {
        public SMSDataModel data { get; set; }
    }

}