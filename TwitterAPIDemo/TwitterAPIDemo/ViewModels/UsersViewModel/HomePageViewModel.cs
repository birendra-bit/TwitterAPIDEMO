using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Network;
using TwitterAPIDemo.ViewModels.Base;
using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        APIservice _aPIservice;
        private ObservableCollection<Tweets> tweetData { get; set; }
        public HomePageViewModel(){}
        public override async Task InitializeAsync(Page page)
        {
            TweetData = new ObservableCollection<Tweets>();
            await usersTweets();
        }
        public ObservableCollection<Tweets> TweetData
        {
            get => tweetData;
            set
            {
                tweetData = value;
                OnPropertyChanged();
            }
        }
        private bool isFresh;
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
        public bool IsFresh
        {
            get => isFresh;
            set
            {
                isFresh = value;
                OnPropertyChanged();
            }
        }
        public Command RefreshData
        {
            get
            {
                return new Command( async() =>
                {
                    TweetData.Clear();
                    await usersTweets();
                    IsFresh = false;
                });
            }
        }
        public async Task usersTweets()
        {
            _aPIservice = new APIservice();
            var url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
            var usersTweets = JsonConvert.DeserializeObject<List<UsersTweets>>(await _aPIservice.GetResponse(url, null));
            if (usersTweets == null)
                return;
            foreach (var data in usersTweets)
            {
                TweetData.Add(new Tweets
                {
                    Name = data.user.name,
                    Uname = data.user.screen_name,
                    ProfileImg = data.user.profile_image_url,
                    TweetText = data.text,
                    TweetMedia = data.user.profile_banner_url
                });
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