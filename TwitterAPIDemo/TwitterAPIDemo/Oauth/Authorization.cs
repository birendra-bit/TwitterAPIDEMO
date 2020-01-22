using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TwitterAPIDemo.Oauth
{
    public class Authorization
    {
        public string oauth_consumer_key = "jVWQH3Qd7rzwrXFpbUnImqwUQ";
        public string consumer_secret = "OHxAhWHrRAlZZeYRvI41liPh01wE4w5Nwa6rQt7D3JszjYKnu8";
        public string oauth_token = "1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi";
        public string oauth_token_secrect = "8RUtH0ZstbYkYzT1OjofLqo10dCTmeTy0XZzuVceTtl1E";
        //public string oauth_signature_method = "HMAC-SHA1";
        //public string oauth_version = "1.0";
        public string oauth_nonce;
        public Int32 oauth_timestamp;
        //public string oauthSignature;
        public Authorization()
        {
            this.oauth_nonce = RandomString();
            this.oauth_timestamp = GetTimeStamp();
            //generateSig();
        }
        public string GetSignatureBaseString(string strUrl,string method, SortedDictionary<string, string> data)
        {
            
            string Signature_Base_String = method;
            Signature_Base_String = Signature_Base_String.ToUpper();
            
            Signature_Base_String = Signature_Base_String + "&";
            
            string PercentEncodedURL = HttpUtility.UrlEncode(strUrl);
            Signature_Base_String = Signature_Base_String + PercentEncodedURL;
            
            Signature_Base_String = Signature_Base_String + "&";
            
            var parameters = new SortedDictionary<string, string>
            {
                {"oauth_consumer_key", oauth_consumer_key},
                { "oauth_token", oauth_token },
                {"oauth_signature_method", "HMAC-SHA1"},
                {"oauth_timestamp", oauth_timestamp.ToString()},
                {"oauth_nonce", oauth_nonce},
                {"oauth_version", "1.0"}
            };
            
            foreach (KeyValuePair<string, string> elt in data)
            {
                parameters.Add(elt.Key, elt.Value);
            }

            bool first = true;
            foreach (KeyValuePair<string, string> elt in parameters)
            {
                if (first)
                {
                    Signature_Base_String = Signature_Base_String + HttpUtility.UrlEncode(elt.Key + "=" + elt.Value);
                    first = false;
                }
                else
                {
                    Signature_Base_String = Signature_Base_String + HttpUtility.UrlEncode("&" + elt.Key + "=" + elt.Value);
                }
            }
            string signingkey =consumer_secret + "&" + oauth_token_secrect;
            return GetSha1Hash( signingkey ,Signature_Base_String);
        }
        public string GetSha1Hash(string key, string baseString )
        {
            var encoding = new System.Text.ASCIIEncoding();

            byte[] keyBytes = encoding.GetBytes(key);
            byte[] messageBytes = encoding.GetBytes(baseString);

            string strSignature = string.Empty;

            using (HMACSHA1 SHA1 = new HMACSHA1(keyBytes))
            {
                var Hashed = SHA1.ComputeHash(messageBytes);
                strSignature = Convert.ToBase64String(Hashed);
            }
            
            return strSignature;
        }
        //public void generateSig()
        //{
        //    var requestType = "POST";
        //    var url = "https://api.twitter.com/1.1/statuses/update.json";
        //    var encodedUrl = HttpUtility.UrlEncode(url);

        //    var parameters = "include_entities=true" +
        //                      "&oauth_consumer_key=" + oauth_consumer_key +
        //                      "&oauth_nonce=" + oauth_nonce +
        //                      "&oauth_signature_method=" + oauth_signature_method +
        //                      "&oauth_token=" + oauth_token +
        //                      "&oauth_timestamp=" + oauth_timestamp +
        //                      "&oauth_version=" + oauth_version +
        //                      "&status=" + "hello";
        //    var parameterEncode = HttpUtility.UrlEncode(parameters);


        //    string baseString = $"{requestType}&{encodedUrl}&{parameterEncode}";

        //    //string baseString = HttpUtility.UrlEncode("POST&" + url + parameters);
        //    string signningKey = consumer_secret + "&" + access_token_key;
        //    oauthSignature = hash_hmac(baseString, signningKey);
        //}
        //public string hash_hmac(string signatureString, string secretKey)
        //{
        //    //HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
        //    //byte[] buffer = Encoding.UTF8.GetBytes(signatureString);
        //    //string result = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
        //    //return Convert.ToBase64String(Encoding.UTF8.GetBytes(result));
        //    var encoding = new System.Text.ASCIIEncoding();

        //    byte[] keyBytes = encoding.GetBytes(secretKey);
        //    byte[] messageBytes = encoding.GetBytes(signatureString);

        //    string strSignature = string.Empty;

        //    using (HMACSHA1 SHA1 = new HMACSHA1(keyBytes))
        //    {
        //        var Hashed = SHA1.ComputeHash(messageBytes);
        //        strSignature = Convert.ToBase64String(Hashed);
        //    }

        //    return strSignature;
        //}
        private Int32 GetTimeStamp()
        {
            return (Int32)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

            //return (Int32)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public string RandomString()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 11)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
