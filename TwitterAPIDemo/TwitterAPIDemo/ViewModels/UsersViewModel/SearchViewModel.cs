using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class SearchViewModel : BaseViewModel
    {
        private List<Details> searchUser;
        public List<Details> SearchUser {
            get {

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
            this.SearchUser = null;           
        }
        public ICommand PerformSearch => new Command<string>((query) =>
        {
                SearchUser = SearchDataList(query);  
        });
                private List<Details> SearchDataList( string name )
        {
            var client = new RestClient("https://api.twitter.com/1.1/users/search.json?q=" +name);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579697362\",oauth_nonce=\"VxwTyUuV5GQ\",oauth_version=\"1.0\",oauth_signature=\"5VIennZI%2Bkn6ZsPWSDvvF7rz%2BrY%3D\"");
            IRestResponse response = client.Execute(request);


            var Users = JsonConvert.DeserializeObject<List<User>>(response.Content);
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
