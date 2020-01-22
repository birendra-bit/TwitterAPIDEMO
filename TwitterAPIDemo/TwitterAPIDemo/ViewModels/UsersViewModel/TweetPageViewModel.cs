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
            var client = new RestClient("https://api.twitter.com/1.1/statuses/update.json");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579697849\",oauth_nonce=\"MTaw79zOTRO\",oauth_version=\"1.0\",oauth_signature=\"ozaNiCRJju2z1sPgQIiBHw%2FZh4c%3D\"");
            request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------434258907633085152871772");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("status", "hey there");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }

        public string Text { get; set; }
    }
}
