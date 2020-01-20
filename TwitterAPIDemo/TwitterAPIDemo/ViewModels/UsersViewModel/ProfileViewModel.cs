using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class ProfileViewModel : BaseViewModel
    {


        ProfilePageModel obj;
        public ProfileViewModel()
        {
            profile();
        }
        private void profile()
        {
            //obj = new ProfilePageModel();
            var client = new RestClient("https://api.twitter.com/1.1/users/show.json?screen_name=ashishchopra01");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("oauth_callback", "https://www.google.com");
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-X3kasuaHuilqbVu7NhrrJYSi7GlkdQ\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579503109\",oauth_nonce=\"qfGRZmWqUIA\",oauth_version=\"1.0\",oauth_signature=\"uXdZsGCEFCp5FcF5SVE3wfYKUFM%3D\"");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            obj = JsonConvert.DeserializeObject<ProfilePageModel>(response.Content);
            Banner = obj.profile_banner_url;
            ProfileImage = obj.profile_image_url;
            Name = obj.name;
            Username = obj.screen_name;
            Location = obj.location;
            Description = obj.description;

            //IRestResponse response = client.Execute(request);




        }
        //public string profile_banner_url { get => banner; set => banner = value; }    
        

        public string Banner { get; set; }



        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }

    }


}
