using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public List<Tweets> tweets { get; set; }
        public HomePageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            Task.Run(()=>usersTweets()).Wait();
        }

        public Command OpenTweetPage
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new TweetPage());
                });
            }
        }

        public async Task usersTweets()
        {
            Authorization auth = new Authorization();
            var url = "https://api.twitter.com/1.1/statuses/home_timeline.json";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, null, "GET"));

                var httpResponse = await httpClient.GetAsync(url);

                if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    DisplayAlert("sorry", "something went wrong", "ok");
                    return;
                }
                var httpContent = await httpResponse.Content.ReadAsStringAsync();

                var usersTweets = JsonConvert.DeserializeObject<List<UsersTweets>>(httpContent);
                List<Tweets> tweets1 = new List<Tweets>();
                foreach (var data in usersTweets)
                {
                    tweets1.Add(new Tweets
                    {
                        Name = data.user.name,
                        Uname = data.user.screen_name,
                        ProfileImg = data.user.profile_image_url,
                        TweetText = data.text,
                        TweetMedia = data.user.profile_banner_url
                        //TweetMedia = data.entities.urls.Count > 0 ?data.entities.urls[0].expanded_url:null
                    });
                }
                this.tweets = tweets1;
            }
        }
        public class Tweets
        {
            public Tweets() { }
            public string Name { get; set; }
            public string Uname { get; set; }
            public string TweetText { get; set; }
            public string TweetMedia { get; set; }
            public string ProfileImg { get; set; }
        }
    }
}
