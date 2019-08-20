using JWT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EasyDefine.Oauth20.Core
{
    public class App
    {
        /// <summary>
        /// 用户的公开权限唯一标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 用户的密匙
        /// </summary>
        public string AppSecret
        {
            get
            {
                return MD5Encrypt(AppKey,32);
            }
        }

        /// <summary>
        /// 私有构造方法
        /// </summary>
        private App() {

        }

        /// <summary>
        /// 创建新的App账户
        /// </summary>
        /// <returns></returns>
        public static App Get(string appkey) {
            var _app = new App();
            _app.AppKey = appkey;
            return _app;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="bit"></param>
        /// <returns></returns>
        private string MD5Encrypt(string password, int bit)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            if (bit == 16)
                return tmp.ToString().Substring(8, 16);
            else
            if (bit == 32) return tmp.ToString();//默认情况
            else return string.Empty;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="AppKey">公开权限唯一标识</param>
        /// <param name="NonceStr">随机串</param>
        /// <param name="CurTime">时间戳</param>
        /// <param name="CheckSum">校验和</param>
        /// <returns></returns>
        public bool Check(string AppKey, string NonceStr, string CurTime, string CheckSum)
        {
            var AppSecret = App.Get(AppKey).AppSecret;// 拿到密匙
            SHA1 sha1 = new SHA1CryptoServiceProvider();//创建SHA1对象
            byte[] bytes_in = Encoding.UTF8.GetBytes(AppSecret + NonceStr + CurTime);//将待加密字符串转为byte类型 byte[] bytes_out = sha1.ComputeHash(bytes_in);//Hash运算 sha1.Dispose();//释放当前实例使用的所有资源 String result = BitConverter.ToString(bytes_out);//将运算结果转为string类型 result = result.Replace("-", "").ToUpper();//替换并转为大写 return result;
            byte[] bytes_out = sha1.ComputeHash(bytes_in);//Hash运算
            sha1.Dispose();//释放当前实例使用的所有资源
            var serverCheckSum = getFormattedText(bytes_out).ToLower();
            return serverCheckSum == CheckSum;
        }

        /// <summary>
        /// 签发Token
        /// </summary>
        /// <returns></returns>
        public string IssueToken(TokenInfo tokenInfo) {
            var jwtcreated =
             Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);
            var jwtcreatedOver =
            Math.Round((DateTime.UtcNow.AddHours(2) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds + 5);
            var payload = new Dictionary<string, dynamic>
                {
                    {"iss", tokenInfo.iss},//非必须。issuer 请求实体，可以是发起请求的用户的信息，也可是jwt的签发者。
                    {"iat", jwtcreated},//非必须。issued at。 token创建时间，unix时间戳格式
                    {"exp", jwtcreatedOver},//非必须。expire 指定token的生命周期。unix时间戳格式
                    {"aud", tokenInfo.aud},//非必须。接收该JWT的一方。
                    {"sub", tokenInfo.sub},//非必须。该JWT所面向的用户
                    {"jti", tokenInfo.jti},//非必须。JWT ID。针对当前token的唯一标识
                    {"UserName", tokenInfo.UserName},//自定义字段 用于存放当前登录人账户信息
                    {"UserPwd", tokenInfo.UserPwd},//自定义字段 用于存放当前登录人登录密码信息
                    {"UserRole", tokenInfo.UserRole},//自定义字段 用于存放当前登录人登录权限信息
                };
            return JsonWebToken.Encode(payload, AppSecret, JwtHashAlgorithm.RS256);
        }

        public TokenInfo DecryptToken(string TokenStr) {
            var tokenobject = JsonConvert.SerializeObject(JsonWebToken.Decode(TokenStr, AppSecret));
            var token = JsonConvert.DeserializeObject<TokenInfo>(tokenobject
                );
            return token;
        }

        private string getFormattedText(byte[] bytes)
        {
            int len = bytes.Length;
            StringBuilder buf = new StringBuilder(len * 2);
            for (int j = 0; j < len; j++)
            {
                buf.Append(HEX_DIGITS[(bytes[j] >> 4) & 0x0f]);
                buf.Append(HEX_DIGITS[bytes[j] & 0x0f]);
            }
            return buf.ToString();
        }

        private static char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5',
            '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
    }
}
