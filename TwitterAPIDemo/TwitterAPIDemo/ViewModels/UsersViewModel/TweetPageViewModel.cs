using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class TweetPageViewModel:BaseViewModel
    {
        public TweetPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
        }
        public Command CloseBtn
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PopAsync();
                });
            }
        }
        public Command TweetBtn {
            get
            {
                return new Command(() => {
                    UpdateStatus(Text);
                    Navigation.PopAsync();
                });
            }
        }

        private void UpdateStatus(string text)
        {
            var client = new RestClient("https://api.twitter.com/1.1/statuses/update.json?status=");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("status", text);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"jVWQH3Qd7rzwrXFpbUnImqwUQ\",oauth_token=\"1165850293965209600-4efdWDjKAlScxCVL9EPi8wy42FiZYi\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579378196\",oauth_nonce=\"tG81H00E4yW\",oauth_version=\"1.0\",oauth_signature=\"E9DIQ9woYJkqJVa9Nbixnv2v2JE%3D\"");
            IRestResponse response = client.Execute(request);
            if( !response.StatusCode.Equals("OK"))
            {
                DisplayAlert("Sorry", "Something went wrong", "Ok");
            }
            Console.WriteLine(response.Content);
        }

        public string Text { get; set; }
    }
}
