using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class FollowerViewModel : BaseViewModel
    {
        private bool apiHit = true;
        public List<Follower> followerList { get; set; }
       
        public FollowerViewModel()
        {
            if (apiHit)
            {
                Task.Run(() => GenerateFollowerList()).Wait();
                apiHit = false;
            }
        }

        private async Task GenerateFollowerList()
        {
            Authorization auth = new Authorization();
            var url = "https://api.twitter.com/1.1/followers/list.json";
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

                var follower = JsonConvert.DeserializeObject<FollowingModel>(httpContent);
                List<Follower> followers = new List<Follower>();
                foreach (var val in follower.users)
                {
                    followers.Add(new Follower
                    {
                        Name = val.name,
                        Uname = val.screen_name,
                        ProfileImgUrl = val.profile_image_url_https,
                        Status = val.following ? "Following" : "follow"
                    });
                }
                this.followerList = followers;
            }
        }

        public class Follower
        {
            public Follower() { }
            public string Name { get; set; }
            public string Uname { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
    }

}
