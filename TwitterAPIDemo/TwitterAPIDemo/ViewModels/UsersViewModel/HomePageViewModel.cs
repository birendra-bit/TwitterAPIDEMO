using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public IList<UsersTweets> tweetsList { get; set; }
        public HomePageViewModel()
        {
            usersTweets();
        }

        private void usersTweets()
        {
            tweetsList = new List<UsersTweets>();
            var client = new RestClient("https://api.twitter.com/1.1/statuses/home_timeline.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("oauth_callback", "http://mobile.twitter.com");
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"jVWQH3Qd7rzwrXFpbUnImqwUQ\",oauth_token=\"1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579183284\",oauth_nonce=\"mUbWN3RoFsX\",oauth_version=\"1.0\",oauth_signature=\"NpD5RxgWsVp4zZxymt9hI7dNO9k%3D\"");

            IRestResponse response = client.Execute(request);
            tweetsList = JsonConvert.DeserializeObject<List<UsersTweets>>(response.Content);
            //int l = tweetsList.Count;
            Debug.WriteLine(response.Content);
            //HttpClient http = new HttpClient();
            //http.Timeout = TimeSpan.FromMinutes(1);
            //http.DefaultRequestHeaders.Add("oauth_callback", "http://mobile.twitter.com");
            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "OAuth oauth_consumer_key=\"jVWQH3Qd7rzwrXFpbUnImqwUQ\",oauth_token=\"1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579158309\",oauth_nonce=\"ls3vWzAeTSF\",oauth_version=\"1.0\",oauth_signature=\"Ze9eADIZDLmEPJdsHaH%2FogA%2Bk3E%3D\"");
            //var resp = await http.GetAsync("https://api.twitter.com/1.1/lists/list.json");
            //Debug.WriteLine(resp);
        }
    }
}
