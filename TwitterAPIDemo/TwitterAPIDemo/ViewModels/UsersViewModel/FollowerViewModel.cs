using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
        public class FollowerViewModel:BaseViewModel
    {
        private bool apiHit = true;
        public List<Follower> followerList { get; set; }
        public FollowerViewModel()
        {
            if( apiHit)
            {
                followerList = GenerateFollowerList();
                apiHit = false;
            }
        }

        private List<Follower> GenerateFollowerList()
        {
            var client = new RestClient("https://api.twitter.com/1.1/followers/list.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"jVWQH3Qd7rzwrXFpbUnImqwUQ\",oauth_token=\"1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579508404\",oauth_nonce=\"0fSi9oCLunG\",oauth_version=\"1.0\",oauth_signature=\"Z%2BFrKEQzNXhE2S8XTzkWIn0aXQ4%3D\"");
            IRestResponse response = client.Execute(request);

            var follower = JsonConvert.DeserializeObject<FollowerModel>(response.Content);
            List<Follower> followers = new List<Follower>();
            foreach( var val in follower.users)
            {
                followers.Add(new Follower {
                    Name = val.name,
                    Uname = val.screen_name,
                    ProfileImgUrl = val.profile_image_url_https,
                    Status = val.following?"Following":"follow"
                });
            }
            Console.WriteLine(response.Content);
            return followers;
        }

        public class Follower
        {
            public Follower(){}
            public string Name { get; set; }
            public string Uname { get; set; }
            public string ProfileImgUrl { get; set; }
            public string Status { get; set; }
        }
    }
    
}
