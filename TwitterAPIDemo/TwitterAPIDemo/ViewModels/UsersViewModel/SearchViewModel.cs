using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Network;
using TwitterAPIDemo.Services;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        APIservice _aPIservice;
        private string _url = string.Empty;
        private ObservableCollection<UserDetails> searchUser;
        public ObservableCollection<UserDetails> SearchUser
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
            this.SearchUser = new ObservableCollection<UserDetails>();
        }
        public ICommand PerformSearch => new Command<string>(async (query) =>
        {
            await SearchDataList(query);
        });
        private async Task SearchDataList(string name)
        {
            try
            {
                _aPIservice = new APIservice();
                _url = "https://api.twitter.com/1.1/users/search.json";
                var searchData = new Dictionary<string, string>
                    {
                        {"q",name}
                    };
                var Users = await _aPIservice.GetResponse<List<User>>(_url, searchData, "q=" + name);
                SearchUser.Clear();
                SearchUser = new ObservableCollection<UserDetails>();
                foreach (var data in Users)
                {
                    SearchUser.Add(new UserDetails
                    {
                        Name = data.name,
                        ScreenName = data.screen_name,
                        ProfileImgUrl = data.profile_image_url,
                        Status = data.following ? "Following" : "Follow"
                    });
                }
            }
            catch (Exception e)
            {
            }
        }
        public Command Follow
        {
            get
            {
                return new Command(CreateOrDestroyUser);
            }
        }
         public override async void CreateOrDestroyUser(object obj)
         {
            string Uname = (string)obj.GetType().GetProperty("Uname").GetValue(obj);

            _aPIservice = new APIservice();
            _url = "https://api.twitter.com/1.1/friendships/create.json";
            Dictionary<string, string> data = new Dictionary<string, string>
                    {
                        { "screen_name", Uname },
                        { "follow", "1" }
                    };
            if (await _aPIservice.PostResponse(_url, data, null) != null)
                DisplayFlashingMessage("user followed successful");
        }
    }
}
