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
        private bool _isReFresh = false;
        private ObservableCollection<Tweets> tweetData { get; set; }
        public HomePageViewModel() { }

        public override async Task InitializeAsync(Page page)
        {
            await base.InitializeAsync(page);
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
        public bool IsReFresh
        {
            get => _isReFresh;
            set
            {
                _isReFresh = value;
                OnPropertyChanged(nameof(IsReFresh));
            }
        }
        public Command RefreshData
        {
            get
            {
                return new Command(RefreshList);
            }
        }
        private async void RefreshList()
        {
            IsReFresh = true;
            TweetData.Clear();
            await usersTweets();
            IsReFresh = false;
        }
        public async Task usersTweets()
        {
            _aPIservice = new APIservice();
            var url = "https://api.twitter.com/1.1/statuses/home_timeline.json";
            var usersTweets = await _aPIservice.GetResponse<List<UsersTweets>>(url, null, string.Empty);
            if (usersTweets == null)
                return;
            foreach (var data in usersTweets)
            {
                TweetData.Add(new Tweets
                {
                    Name = data.user.name,
                    ScreenName = data.user.screen_name,
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
            public string ScreenName { get; set; }
            public string TweetText { get; set; }
            public string TweetMedia { get; set; }
            public string ProfileImg { get; set; }
        }
    }
}