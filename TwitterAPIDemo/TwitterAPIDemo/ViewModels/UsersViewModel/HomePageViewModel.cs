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
        public IList<Tweets> tweets { get; set; }
        public HomePageViewModel()
        {
            this.tweets= usersTweets();
        }
        public List<Tweets> usersTweets()
        {
            
            var client = new RestClient("https://api.twitter.com/1.1/statuses/home_timeline.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("oauth_callback", callBackUrl);
            request.AddHeader("Authorization", authorization);

            IRestResponse response = client.Execute(request);
            var usersTweets = JsonConvert.DeserializeObject<List<UsersTweets>>(response.Content);
            List<Tweets> tweets = new List<Tweets>();
           foreach( var data in usersTweets)
            {
                Debug.WriteLine(data.user.name,data.user.screen_name);
                tweets.Add(new Tweets
                {
                    Name = data.user.name,
                    Uname = data.user.screen_name,
                    ProfileImg = data.user.profile_image_url,
                    TweetText = data.text,
                    TweetMedia = data.user.profile_banner_url
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
