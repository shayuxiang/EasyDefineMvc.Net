using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDefine.Oauth20.Core
{
    public class TokenInfo
    {
        public TokenInfo()
        {
            iss = "签发者信息";
            aud = "http://example.com";
            sub = "HomeCare.VIP";
            jti = DateTime.Now.ToString("yyyyMMddhhmmss");
            UserName = "ppm_online_web";
            UserPwd = "14dddbf2412a457caf8141e4b2e787d0";
            UserRole = "1550816425";
        }
        public string iss { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string jti { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string UserRole { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
