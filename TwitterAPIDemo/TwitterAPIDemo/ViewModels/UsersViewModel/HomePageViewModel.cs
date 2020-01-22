using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;
using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public IList<Tweets> tweets { get; set; }
        public HomePageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            this.tweets= usersTweets();
        }
        public Command OpenTweetPage
        {
            get{
                return new Command(() => {
                    Navigation.PushAsync(new TweetPage());
                });
            }
        }
        public List<Tweets> usersTweets()
        {
            
            var client = new RestClient("https://api.twitter.com/1.1/statuses/home_timeline.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"jVWQH3Qd7rzwrXFpbUnImqwUQ\",oauth_token=\"1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579606608\",oauth_nonce=\"0hwSdR4cMTL\",oauth_version=\"1.0\",oauth_signature=\"Qj%2Bs5ymuaqHeLgYse3PhCw3kiKc%3D\"");

            IRestResponse response = client.Execute(request);
            
            var usersTweets = JsonConvert.DeserializeObject<List<UsersTweets>>(response.Content);
            List<Tweets> tweets = new List<Tweets>();
           foreach( var data in usersTweets)
            {
                tweets.Add(new Tweets
                {
                    Name = data.user.name,
                    Uname = data.user.screen_name,
                    ProfileImg = data.user.profile_image_url,
                    TweetText = data.text,
                    TweetMedia = data.user.profile_banner_url
                    //TweetMedia = data.entities.urls.Count > 0 ?data.entities.urls[0].expanded_url:null
                });
            }
            return tweets;
        }
        public class Tweets
        {
            public Tweets(){}
            public string Name { get; set ; }
            public string Uname { get; set; }
            public string TweetText { get; set; }
            public string TweetMedia { get; set; }
            public string ProfileImg { get; set; }
        }
    }
}
