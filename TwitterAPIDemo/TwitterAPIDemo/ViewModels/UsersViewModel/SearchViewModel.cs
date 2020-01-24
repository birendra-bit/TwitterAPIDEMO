using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<Details> searchUser;
        //private object _consumerKeySecret;
        //private object _accessTokenSecret;

        public ObservableCollection<Details> SearchUser
        {
            get
            {
                return searchUser;
            }
            set
            {
                searchUser = value;
                OnPropertyChanged();
            }
        }
        public SearchViewModel()
        {
            this.SearchUser = new ObservableCollection<Details>();
        }
        public ICommand PerformSearch => new Command<string>((query) =>
        {
            var users = SearchDataList(query);

            SearchUser.Clear();
            if(users == null)
            {
                return;
            }
            //(System.NullReferenceException)
            users.ForEach(a => SearchUser.Add(a));
        });

        //public string CreateSignature(string url, string timestamp, string authNonce)
        //{
        //    //string builder will be used to append all the key value pairs
        //    var stringBuilder = new StringBuilder();
        //    stringBuilder.Append("GET&");
        //    stringBuilder.Append(Uri.EscapeDataString(url));
        //    stringBuilder.Append("&");
            
        //    //the key value pairs have to be sorted by encoded key
        //    var dictionary = new SortedDictionary<string, string>
        //                         {
        //                             {"oauth_version", "1.0"},
        //                             {"oauth_consumer_key", "Cf1w0izou1SdsMCq7M4wAewlH"},
        //                             {"oauth_nonce", authNonce},
        //                             {"oauth_signature_method", "HMAC-SHA1"},
        //                             {"oauth_timestamp", timestamp},
        //                             {"oauth_token", "1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu"}
        //                         };

        //    foreach (var keyValuePair in dictionary)
        //    {
        //        //append a = between the key and the value and a & after the value
        //        stringBuilder.Append(Uri.EscapeDataString(string.Format("{0}={1}&", keyValuePair.Key, keyValuePair.Value)));
        //    }
        //    string signatureBaseString = stringBuilder.ToString().Substring(0, stringBuilder.Length - 3);

        //    //generation the signature key the hash will use
        //    string signatureKey =
        //        Uri.EscapeDataString("u6TPTIL7SaDgMxSqrX88dlNZD3g8nmFJzT6JraPCauPGcu3KHr") + "&" +
        //        Uri.EscapeDataString("SaghA1BHFhiro7fmLGnqviTxyu1i5YdV3EKpefkcKqyKv");

        //    var hmacsha1 = new HMACSHA1(
        //        new ASCIIEncoding().GetBytes(signatureKey));

        //    //hash the values
        //    string signatureString = Convert.ToBase64String(
        //        hmacsha1.ComputeHash(
        //            new ASCIIEncoding().GetBytes(signatureBaseString)));

        //    return signatureString;
        //}

        private List<Details> SearchDataList(string name)
        {
            string url = "https://api.twitter.com/1.1/users/search.json?q=" + name;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
            {
                DisplayAlert("Alert from SearchViewModel", "", "OK");
                return null;
            }
            if (response.StatusCode.Equals(System.Net.HttpStatusCode.BadRequest))
            {
                DisplayAlert("Alert from SearchViewModel", "", "OK");
                return null;
            }


            var Users = JsonConvert.DeserializeObject<List<User>>(response.Content);
            List<Details> SearchResult = new List<Details>();
            foreach (var data in Users)
            {
                SearchResult.Add(new Details
                {
                    Name = data.name,
                    Username = data.screen_name,
                    ProfileImgUrl = data.profile_image_url,
                    Status = "Follow"
                });
            }
            return SearchResult;
        }
        public Command Follow
        {
            get
            {
                return new Command(() =>
                {

                });
            }
        }
        public class Details
        {
            public Details() { }
            public string Name { get; set; }
            public string Username { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
        //string authNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(
        //    DateTime.Now.Ticks.ToString()));
        //var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //var timestamp = Convert.ToInt64(timeSpan.TotalSeconds);
        //const string headerFormat = "OAuth oauth_consumer_key=\"{0}\", " +
        //                    "oauth_nonce=\"{1}\", " +
        //                    "oauth_signature=\"{2}\", " +
        //                    "oauth_signature_method=\"{3}\", " +
        //                    "oauth_timestamp=\"{4}\", " +
        //                    "oauth_token=\"{5}\", " +
        //                    "oauth_version=\"{6}\"";

        //var authHeader = string.Format(headerFormat,
        //                        Uri.EscapeDataString("Cf1w0izou1SdsMCq7M4wAewlH"),
        //                        Uri.EscapeDataString(authNonce),
        //                        Uri.EscapeDataString(CreateSignature(url, timestamp.ToString(), authNonce)),
        //                        Uri.EscapeDataString("HMAC-SHA1"),
        //                        Uri.EscapeDataString(timestamp.ToString()),
        //                        Uri.EscapeDataString("1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu"),
        //                        Uri.EscapeDataString("1.0")
        //                );
        //request.AddHeader("Authorization", authHeader);
        //request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------814731670592730849067205");
        //request.AlwaysMultipartFormData = true;


    }
}
