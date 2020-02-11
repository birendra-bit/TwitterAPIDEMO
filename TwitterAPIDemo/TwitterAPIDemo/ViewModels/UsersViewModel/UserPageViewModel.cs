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
    public class UserPageViewModel : BaseViewModel
    {
        APIservice _aPIservice;
        private bool _apiHit = true;
        private bool _isFollowerVisible;
        private bool _isFollowingVisible;
        private string _url = string.Empty;
        private ObservableCollection<UserDetails> followerList { get; set; }
        public ObservableCollection<UserDetails> FollowerList
        {
            get => followerList;
            set
            {
                followerList = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<UserDetails> followingList { get; set; }
        public ObservableCollection<UserDetails> FollowingList
        {
            get => followingList;
            set
            {
                followingList = value;
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
            Default();
            if (_apiHit)
            {
                FollowerList = await GenerateFollowerList();
                
                FollowingList = await GenerateFollowingList();
                _apiHit = false;
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
                return new Command(() =>
                {
                    IsFollowerVisible = true;
                    IsFollowingVisible = false;
                    BgColorFollower = "#00bfff";
                    BgColorFollowing = "Transparent";
                });
            }
        }
        public Command DisplayFollowing
        {
            get
            {
                return new Command(() =>
                {
                    IsFollowingVisible = true;
                    IsFollowerVisible = false;
                    BgColorFollower = "Transparent";
                    BgColorFollowing = "#00bfff";
                });
            }
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
        public Command Unfollow
        {
            get { return new Command(UnfollowUser); }
        }
        public Command CreateOrDestroyFriends
        {
            get
            {
                return new Command(CreateOrDestroyFriend);
            }
        }
        public async void CreateOrDestroyFriend(object obj)
        {
            string ScreenName = (string)obj.GetType().GetProperty("ScreenName").GetValue(obj);
            string status = (string)obj.GetType().GetProperty("Status").GetValue(obj);
            _aPIservice = new APIservice();

            Dictionary<string, string> data;
            if (status == "follow")
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
                _url = "https://api.twitter.com/1.1/friendships/destroy.json";
                data = new Dictionary<string, string>
                    {
                        { "screen_name", ScreenName }
                    };
            }
            if (await _aPIservice.PostResponse(_url, data, null) == null)
                return;
            FollowerList.Clear();
            FollowerList = await GenerateFollowerList();
            DependencyService.Get<iMessage>().Shorttime("Action successful");
        }
        private async void UnfollowUser(object obj)
        {
            string ScreenName = (string)obj.GetType().GetProperty("ScreenName").GetValue(obj);
            if (!await DisplalertAlertWithResponse((string)obj.GetType().GetProperty("Name").GetValue(obj), "You want to unfollow?", "Yes", "No"))
            {
                return;
            }
            _aPIservice = new APIservice();
            _url = "https://api.twitter.com/1.1/friendships/destroy.json";
            var data = new Dictionary<string, string>
                {
                    { "screen_name", ScreenName }
                };

            if (await _aPIservice.PostResponse(_url, data, null) == null)
                return;
            FollowingList.Clear();
            FollowingList = await GenerateFollowingList();
            DependencyService.Get<iMessage>().Shorttime("unfollow " + ScreenName + " successful");
        }
        private async Task<ObservableCollection<UserDetails>> GenerateFollowerList()
        {
            _url = "https://api.twitter.com/1.1/followers/list.json";
            _aPIservice = new APIservice();
            followerList = new ObservableCollection<UserDetails>();
            var follower = JsonConvert.DeserializeObject<FollowingModel>(await _aPIservice.GetResponse(_url, null));
            foreach (var val in follower.users)
            {
                followerList.Add(new UserDetails
                {
                    Name = val.name,
                    ScreenName = val.screen_name,
                    ProfileImgUrl = val.profile_image_url_https,
                    Status = val.following ? "Following" : "follow"
                });
            }
            return followerList;
        }
        private async Task<ObservableCollection<UserDetails>> GenerateFollowingList()
        {
            _url = "https://api.twitter.com/1.1/friends/list.json";

            _aPIservice = new APIservice();
            followingList = new ObservableCollection<UserDetails>();
            var following = JsonConvert.DeserializeObject<FollowingModel>(await _aPIservice.GetResponse(_url, null));
            foreach (var data in following.users)
            {
                followingList.Add(new UserDetails
                {
                    Name = data.name,
                    ScreenName = data.screen_name,
                    ProfileImgUrl = data.profile_image_url_https,
                    Status = "following"
                });
            }
            return followingList;
        }
        public class UserDetails
        {
            public UserDetails() { }
            public string Name { get; set; }
            public string ScreenName { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
    }
}
