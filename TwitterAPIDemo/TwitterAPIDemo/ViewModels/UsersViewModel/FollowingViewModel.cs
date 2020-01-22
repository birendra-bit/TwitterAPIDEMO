using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class FollowingViewModel : BaseViewModel
    {
        private bool apiHit = true;
        public List<Following> followingsList { get; set; }
        public FollowingViewModel()
        {
            if (apiHit)
            {
                this.followingsList = GenerateList();
                apiHit = false;
            }
        }
        private List<Following> GenerateList()
        {
            var client = new RestClient("https://api.twitter.com/1.1/friends/list.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579697265\",oauth_nonce=\"LNlIC3huxGG\",oauth_version=\"1.0\",oauth_signature=\"U%2F2M4B%2BYSCVx69g2EJy7NEbP4P0%3D\"");
            IRestResponse response = client.Execute(request);

            if (!response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                DisplayAlert("Sorry", "Something went wrong", "OK");
                return null;
            }
            var following = JsonConvert.DeserializeObject<FollowingModel>(response.Content);
            List<Following> followingsUser = new List<Following>();
            foreach (var data in following.users)
            {
                followingsUser.Add(new Following
                {
                    Name = data.name,
                    Uname = data.screen_name,
                    ProfileImgUrl = data.profile_image_url_https,
                    Status = "following"
                });
            }
            return followingsUser;
        }
        public class Following
        {
            public Following() { }
            public string Name { get; set; }
            public string Uname { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
    }
}
