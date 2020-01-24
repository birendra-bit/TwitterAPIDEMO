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
            text = auth.CutTweetToLimit(text);
            var data = new Dictionary<string, string>
            {
                { "status", text },
                { "trim_user", "1" },
                { "media_ids", string.Empty}
            };
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, data, "POST"));

                var httpResponse = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));
                if (!httpResponse.IsSuccessStatusCode)
                {
                    DisplayAlert("sorry", "something went wrong", "ok");
                    return;
                }
                var httpContent = await httpResponse.Content.ReadAsStringAsync();
            }
        }
        public string Text { get; set; }
    }
}
