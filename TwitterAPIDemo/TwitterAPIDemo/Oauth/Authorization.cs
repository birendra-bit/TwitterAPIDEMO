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
        readonly string _consumerKey;
        readonly string _consumerKeySecret;
        readonly string _accessToken;
        readonly string _accessTokenSecret;
        readonly HMACSHA1 _sigHasher;
        readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        readonly int _limit;

        public Authorization()
        {
            _consumerKey = "KkxKgkRtuqlPr3soX0agl86oL";
            _consumerKeySecret = "ulxHokYPyF8QC6IOzTqAHyOyv55LhaamMGwBvY4ZDhLAACjlyG";
            _accessToken = "1165850293965209600-y5Muf29Z8wxnBc50YyhJdbOCD1wW8f";
            _accessTokenSecret = "re30nRG9lt7jiuV64iDpOZCFoojujpFdWODqL0GVxSXlC";
            _limit = 280;

            _sigHasher = new HMACSHA1(
                new ASCIIEncoding().GetBytes($"{_consumerKeySecret}&{_accessTokenSecret}")
            );
        }
        public string PrepareOAuth(string URL, Dictionary<string, string> data, string httpMethod)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

            // Add all the OAuth headers we'll need to use when constructing the hash
            Dictionary<string, string> oAuthData = new Dictionary<string, string>();
            oAuthData.Add("oauth_consumer_key", _consumerKey);
            oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
            oAuthData.Add("oauth_timestamp", timestamp.ToString());
            oAuthData.Add("oauth_nonce", Guid.NewGuid().ToString());
            oAuthData.Add("oauth_token", _accessToken);
            oAuthData.Add("oauth_version", "1.0");

            if (data != null) // add text data too, because it is a part of the signature
            {
                foreach (var item in data)
                {
                    oAuthData.Add(item.Key, item.Value);
                }
            }

            // Generate the OAuth signature and add it to our payload
            oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData, httpMethod));

            // Build the OAuth HTTP Header from the data
            return GenerateOAuthHeader(oAuthData);
        }
        private string GenerateSignature(string url, Dictionary<string, string> data, string httpMethod)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format("{0}&{1}&{2}",
                httpMethod,
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString()
                )
            );

            return Convert.ToBase64String(
                _sigHasher.ComputeHash(
                    new ASCIIEncoding().GetBytes(fullSigData.ToString())
                )
            );
        }
        private string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Format(
                "OAuth {0}",
                string.Join(
                    ", ",
                    data
                        .Where(kvp => kvp.Key.StartsWith("oauth_"))
                        .Select(
                            kvp => string.Format("{0}=\"{1}\"",
                            Uri.EscapeDataString(kvp.Key),
                            Uri.EscapeDataString(kvp.Value)
                            )
                        ).OrderBy(s => s)
                    )
                );
        }
        public string CutTweetToLimit(string tweet)
        {
            while (tweet.Length >= _limit)
            {
                tweet = tweet.Substring(0, tweet.LastIndexOf(" ", StringComparison.Ordinal));
            }
            return tweet;
        }
        //public string oauth_consumer_key = "KkxKgkRtuqlPr3soX0agl86oL";
        //public string consumer_secret = "ulxHokYPyF8QC6IOzTqAHyOyv55LhaamMGwBvY4ZDhLAACjlyG";
        //public string oauth_token = "1165850293965209600-y5Muf29Z8wxnBc50YyhJdbOCD1wW8f";
        //public string oauth_token_secrect = "re30nRG9lt7jiuV64iDpOZCFoojujpFdWODqL0GVxSXlC";
        //public string oauth_nonce;
        //public Int32 oauth_timestamp;
        //public Authorization()
        //{
        //    this.oauth_nonce = RandomString();
        //    this.oauth_timestamp = GetTimeStamp();
        //}
        //public string GetSignatureBaseString(string strUrl,string method, SortedDictionary<string, string> data)
        //{

        //    string Signature_Base_String = method;
        //    Signature_Base_String = Signature_Base_String.ToUpper();

        //    Signature_Base_String = Signature_Base_String + "&";

        //    string PercentEncodedURL = HttpUtility.UrlEncode(strUrl);
        //    Signature_Base_String = Signature_Base_String + PercentEncodedURL;

        //    Signature_Base_String = Signature_Base_String + "&";

        //    var parameters = new SortedDictionary<string, string>
        //    {
        //        {"oauth_consumer_key", oauth_consumer_key},
        //        { "oauth_token", oauth_token },
        //        {"oauth_signature_method", "HMAC-SHA1"},
        //        {"oauth_timestamp", oauth_timestamp.ToString()},
        //        {"oauth_nonce", oauth_nonce},
        //        {"oauth_version", "1.0"}
        //    };

        //    foreach (KeyValuePair<string, string> elt in data)
        //    {
        //        parameters.Add(elt.Key, elt.Value);
        //    }

        //    bool first = true;
        //    foreach (KeyValuePair<string, string> elt in parameters)
        //    {
        //        if (first)
        //        {
        //            Signature_Base_String = Signature_Base_String + HttpUtility.UrlEncode(elt.Key + "=" + elt.Value);
        //            first = false;
        //        }
        //        else
        //        {
        //            Signature_Base_String = Signature_Base_String + HttpUtility.UrlEncode("&" + elt.Key + "=" + elt.Value);
        //        }
        //    }
        //    string signingkey =consumer_secret + "&" + oauth_token_secrect;
        //    return GetSha1Hash( signingkey ,Signature_Base_String);
        //}
        //public string GetSha1Hash(string key, string baseString )
        //{
        //    var encoding = new System.Text.ASCIIEncoding();

        //    byte[] keyBytes = encoding.GetBytes(key);
        //    byte[] messageBytes = encoding.GetBytes(baseString);

        //    string strSignature = string.Empty;

        //    using (HMACSHA1 SHA1 = new HMACSHA1(keyBytes))
        //    {
        //        var Hashed = SHA1.ComputeHash(messageBytes);
        //        strSignature = Convert.ToBase64String(Hashed);
        //    }

        //    return strSignature;
        //}
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
        //private Int32 GetTimeStamp()
        //{
        //    return (Int32)new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

        //    //return (Int32)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        //}

        //public string RandomString()
        //{
        //    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        //    var random = new Random();
        //    return new string(Enumerable.Repeat(chars, 11)
        //        .Select(s => s[random.Next(s.Length)]).ToArray());
        //}
    }
}
