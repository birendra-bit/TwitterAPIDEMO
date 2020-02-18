using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Network;
using TwitterAPIDemo.Services;
using TwitterAPIDemo.ViewModels.Base;
using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class UserPageViewModel : BaseViewModel
    {
        APIservice _aPIservice;
        private bool _apiHit = true;
        private bool _isFollowerVisible;
        private bool _isFollowingVisible;
        private string _url = string.Empty;
        private bool _isFollowerRefreshing=false;
        private bool _isFollowRefreshing = false;
        private ObservableCollection<UserDetails> _userList { get; set; }
        public ObservableCollection<UserDetails> FollowerList
        {
            get => _userList;
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<UserDetails> followingList { get; set; }
        public ObservableCollection<UserDetails> FollowingList
        {
            get => _userList;
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }
        private string bgColorFollower;
        private string bgColorFollowing;
        public bool IsFollowerVisible
        {
            get => _isFollowerVisible;
            set { _isFollowerVisible = value; OnPropertyChanged(); }
        }
        public bool IsFollowingVisible
        {
            get => _isFollowingVisible;
            set { _isFollowingVisible = value; OnPropertyChanged(); }
        }
        public UserPageViewModel(){}
        public override async Task InitializeAsync(Page page)
        {
            await base.InitializeAsync(page);
            Default();
            if (_apiHit)
            {
                _url = "https://api.twitter.com/1.1/followers/list.json";
                FollowerList = await GenerateList(_url);
                _url = "https://api.twitter.com/1.1/friends/list.json";
                FollowingList = await GenerateList(_url);
                _apiHit = false;
            }
        }
        public bool IsFollowerRefreshing
        {
            get => _isFollowerRefreshing;
            set
            {
                _isFollowerRefreshing = value;
                OnPropertyChanged(nameof(IsFollowerRefreshing));
            }
        }
        public Command RefreshFollower
        {
            get
            {
                return new Command(RefreshFollowerList);
            }
        }
        public bool IsFollowRefreshing {
            get => _isFollowRefreshing;
            set {
                _isFollowRefreshing = value;
                OnPropertyChanged(nameof(IsFollowRefreshing));
                  }
        }
        public Command RefreshFollowing
        {
            get
            {
                return new Command(RefreshFollowingList);
            }
        }
        public Command OpenSearchPage
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new SearchUserPage());
                });
            }
        }
        public Command DisplayFollower
        {
            get
            {
                return new Command(FollowerLabelStyle);
            }
        }
        public Command DisplayFollowing
        {
            get
            {
                return new Command(FollowingLabelStyle);
            }
        }
        public Command CreateOrDestroyFriends
        {
            get
            {
                return new Command(CreateOrDestroyUser);
            }
        }
        private void FollowingLabelStyle()
        {
            IsFollowingVisible = true;
            BgColorFollower = "Transparent";
            BgColorFollowing = "#00bfff";
            IsFollowerVisible = false;
        }
        private void FollowerLabelStyle()
        {
            IsFollowerVisible = true;
            IsFollowingVisible = false;
            BgColorFollower = "#00bfff";
            BgColorFollowing = "Transparent";
        }
        public string BgColorFollower
        {
            get => bgColorFollower;
            set { bgColorFollower = value; OnPropertyChanged(); }
        }
        public string BgColorFollowing
        {
            get => bgColorFollowing;
            set { bgColorFollowing = value; OnPropertyChanged(); }
        }
        private void Default()
        {
            IsFollowerVisible = true;
            BgColorFollower = "#00bfff";
        }
        private async void RefreshFollowingList()
        {
            IsFollowRefreshing = true;
            _url = "https://api.twitter.com/1.1/friends/list.json";
            FollowingList.Clear();
            FollowingList = await GenerateList(_url);
            IsFollowRefreshing = false;
        }
        private async void RefreshFollowerList()
        {
            IsFollowerRefreshing = true;
            _url = "https://api.twitter.com/1.1/followers/list.json";
            FollowerList.Clear();
            FollowerList = await GenerateList(_url);
            IsFollowerRefreshing = false;
        }
        public override async void CreateOrDestroyUser(object obj)
        {
            string ScreenName = (string)obj.GetType().GetProperty("ScreenName").GetValue(obj);
            string status = (string)obj.GetType().GetProperty("Status").GetValue(obj);
            _aPIservice = new APIservice();

            Dictionary<string, string> data;
            if (status == "Follow")
            {
                if (!await DisplalertAlertWithResponse((string)obj.GetType().GetProperty("Name").GetValue(obj), "You want to follow?", "Yes", "No"))
                {
                    return;
                }
                _url = "https://api.twitter.com/1.1/friendships/create.json";
                data = new Dictionary<string, string>
                    {
                        { "screen_name", ScreenName },
                        { "follow", "1" }
                    };
            }
            else
            {
                if (!await DisplalertAlertWithResponse((string)obj.GetType().GetProperty("Name").GetValue(obj), "You want to unfollow?", "Yes", "No"))
                {
                    return;
                }
                _url = "https://api.twitter.com/1.1/friendships/destroy.json";
                data = new Dictionary<string, string>
                    {
                        { "screen_name", ScreenName }
                    };
            }
            if (await _aPIservice.PostResponse(_url, data, null) == null)
                return;
            if( IsFollowerVisible)
            {
                FollowerList.Clear();
                _url = "https://api.twitter.com/1.1/followers/list.json";
                FollowerList = await GenerateList(_url);
            }
            else
            {
                FollowingList.Clear();
                _url = "https://api.twitter.com/1.1/friends/list.json";
                FollowingList = await GenerateList(_url);
            }
         
            DisplayFlashingMessage("Action successful");
        }
        private async Task<ObservableCollection<UserDetails>> GenerateList(string url)
        {
            try
            {
                _aPIservice = new APIservice();
                var follower = await _aPIservice.GetResponse<FollowingModel>(url, null, string.Empty);
                ObservableCollection<UserDetails> userList = new ObservableCollection<UserDetails>();
                foreach (var val in follower.users)
                {
                    userList.Add(new UserDetails
                    {
                        Name = val.name,
                        ScreenName = val.screen_name,
                        ProfileImgUrl = val.profile_image_url_https,
                        Status = val.following ? "Following" : "Follow"
                    });
                }
                return userList;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
