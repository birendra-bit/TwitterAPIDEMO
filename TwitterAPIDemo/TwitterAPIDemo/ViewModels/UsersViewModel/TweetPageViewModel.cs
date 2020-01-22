using Newtonsoft.Json;
using Plugin.Media;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class TweetPageViewModel : BaseViewModel
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
        public Command TweetBtn
        {
            get
            {
                return new Command(() =>
                {
                    UpdateStatus(Text);
                    Navigation.PopAsync();
                });
            }
        }
        private string sourceImg;
        public string SourceImg {
            get { return sourceImg; }
            set {
                if (sourceImg != value){
                    sourceImg = value;
                    OnPropertyChanged();
                };
            }
        }
        public ICommand UploadImage
        {
            get
            {
                return new Command(async () =>
                {
                    SourceImg = await PickPhoto();
                });
            }
        }

        private async Task<string> PickPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return null;
            }
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });
            if (file == null)
                return null;
            return file.Path;
        }

        private async void UpdateStatus(string text)
        {
            Authorization auth = new Authorization();
            var url = "https://api.twitter.com/1.1/statuses/update.json";

            var data = new SortedDictionary<string, string>
            {
                {"include_entities","true"},
                {"status", text }
                //{"in_reply_to_status_id","" },
                //{"auto_populate_reply_metadata","" },
                //{"exclude_reply_user_ids","" },
                //{"attachment_url","" },
                //{"media_ids","" },
                //{"possibly_sensitive","" },
                //{"lat","" },
                //{"long","" },
                //{"place_id","" },
                //{"display_coordinates","" },
                //{"trim_user", ""},
                //{"enable_dmcommands","" },
                //{"fail_dmcommands","" },
                //{"card_uri","" }
            };
                        
            string signature = auth.GetSignatureBaseString(url, "POST", data);
            var client = new HttpClient();

            Post userData = new Post();
            userData.status = text;
            var json = JsonConvert.SerializeObject(userData);
            var request = new StringContent(json, Encoding.UTF8, "application/json");
            //request.AddParameter("status", text);
            
            string str = HttpUtility.UrlEncode(signature);
            //client.DefaultRequestHeaders.Add("Authorization", "OAuth oauth_consumer_key=\"" + auth.oauth_consumer_key + "\",oauth_token=\"" + auth.oauth_token + "\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"" + auth.oauth_timestamp + "\",oauth_nonce=\"" + auth.oauth_nonce + "\",oauth_vesrion=\"1.0\",oauth_signature=\"" + HttpUtility.UrlEncode(signature)+ "\"");
            //request.AddHeader("Authorization", $"OAuth oauth_consumer_key={auth.oauth_consumer_key},oauth_token={auth.oauth_token},oauth_signature_method={auth.oauth_signature_method},oauth_timestamp={auth.oauth_timestamp},oauth_nonce={auth.oauth_nonce},oauth_version={auth.oauth_version},oauth_signature={auth.oauthSignature}");

            //client.DefaultRequestHeaders.Add("oauth_token", auth.oauth_token);
            //client.DefaultRequestHeaders.Add("oauth_version", "1.0");
            //client.DefaultRequestHeaders.Add("oauth_timestamp", auth.oauth_timestamp.ToString());
            //client.DefaultRequestHeaders.Add("oauth_consumer_key", auth.oauth_consumer_key);
            //client.DefaultRequestHeaders.Add("oauth_nounce", auth.oauth_nonce);
            //client.DefaultRequestHeaders.Add("oauth_signature", HttpUtility.UrlEncode(signature));
            //client.DefaultRequestHeaders.Add("oauth_signature_method", "HMAC-SHA1");


            var response = await client.PostAsync(url,request);

            if (!response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                DisplayAlert("Sorry", "Something went wrong", "Ok");
            }
            Console.WriteLine(response.Content);
        }
        private class Post
        {
            public string status { get; set; }
            public Post(){}
        }
        public string Text { get; set; }
    }
}
