using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private ObservableCollection<Details> searchUser;

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
        public ICommand PerformSearch => new Command<string>(async (query) =>
        {
            var users = await SearchDataList(query);

            SearchUser.Clear();
            if (users == null)
            {
                return;
            }
            //(System.NullReferenceException)
            users.ForEach(a => SearchUser.Add(a));
        });
        private async Task<List<Details>> SearchDataList(string name)
        {
            try
            {
                using ( var httpClient = new HttpClient()){

                    Authorization auth = new Authorization();
                    string url = "https://api.twitter.com/1.1/users/search.json";
                    var data1 = new Dictionary<string, string>
                    {
                        {"q",name}
                    };

                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data1, "GET"));

                    UriBuilder builder = new UriBuilder(url);
                    builder.Query = "q="+name;

                    var httpResponse = httpClient.GetAsync(builder.Uri).Result;
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return null;
                    }
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    var Users = JsonConvert.DeserializeObject<List<User>>(httpContent);

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
            }
            catch(Exception e) {
                return null;
            }
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
    }
}
