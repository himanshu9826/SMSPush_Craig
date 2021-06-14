using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS.Creator.Common.Constant
{
    public class ZohoAPIUrlConstant
    {
            public const string PostSMS = "https://creatorapp.zoho.com/api/v2/simplyfleet/simplyfleet/form/SMS";
            public const string GenerateAccessTokenURL = "https://accounts.zoho.com/oauth/v2/token?refresh_token=[refresh-token]&client_id=[client-id]&client_secret=[client-secret]&grant_type=refresh_token";
        }
    
}