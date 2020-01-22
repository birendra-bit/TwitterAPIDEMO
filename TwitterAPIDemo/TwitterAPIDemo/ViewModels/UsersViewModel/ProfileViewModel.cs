using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;
using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        public INavigation navigation;
        ProfilePageModel obj;
        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            profile();
        }
        public Command EditProfile
        {
            get
            {
                return new Command(() =>
                {
                    //Navigation.PushAsync(new EditProfile());
                    UpdateName(Name);
                });
            }
        }

        private void UpdateName(string name)
        {
            string url = "https://api.twitter.com/1.1/account/update_profile.json?"+"name=" + name;
            PostApi(url);
        }
        //post
        private void PostApi(string url)
        {
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-X3kasuaHuilqbVu7NhrrJYSi7GlkdQ\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579529330\",oauth_nonce=\"JQtfukuBqQL\",oauth_version=\"1.0\",oauth_signature=\"HMta6hC75MxNKfprvcVX4T6FGiM%3D\"");
            request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------306805999221266378467087");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        } 
        
        
        private void profile()
        {
            var client = new RestClient("https://api.twitter.com/1.1/users/show.json?screen_name=ashishchopra01");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579697310\",oauth_nonce=\"R4tgdF4PiQ4\",oauth_version=\"1.0\",oauth_signature=\"syp4hZhWD1H3YQ7R%2BPtx4dA%2BBRs%3D\"");
            IRestResponse response = client.Execute(request);

            obj = JsonConvert.DeserializeObject<ProfilePageModel>(response.Content);
            Banner = obj.profile_banner_url;
            ProfileImage = obj.profile_image_url;
            Name = obj.name;
            Username = obj.screen_name;
            Location = obj.location;
            Description = obj.description;
        }
          
        

        public string Banner { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }
    }

}
