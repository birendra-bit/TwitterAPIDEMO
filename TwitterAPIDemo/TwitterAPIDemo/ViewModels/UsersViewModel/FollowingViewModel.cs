﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
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
                Task.Run(() => GenerateList()).Wait();
                apiHit = false;
            }
        }
        private async void  GenerateList()
        {
            Authorization auth = new Authorization();
            var url = "https://api.twitter.com/1.1/friends/list.json";
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
                var following = JsonConvert.DeserializeObject<FollowingModel>(httpContent);
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
               this.followingsList = followingsUser;
            }
               
            
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
