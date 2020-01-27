using Newtonsoft.Json;
using Plugin.Media;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Oauth;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        public INavigation navigation;
        ProfilePageModel obj;
       // string authToken = "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1580108411\",oauth_nonce=\"u8Sthm6oMRF\",oauth_version=\"1.0\",oauth_signature=\"IkN6Upr0MMzdQoBSgcAqkfNMb%2FA%3D\""; 

        public ProfileViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            Task.Run(() => Profile()).Wait();
        }

        //public override Task InitializeAsync(Page page)
        //{
        //    return base.InitializeAsync(page);
        //}

        //public Command EditProfile
        //{
        //    get
        //    {
        //        return new Command(() =>
        //        {
        //            //Navigation.PushAsync(new EditProfile());
        //            UpdateName(Name);
        //        });
        //    }
        //}
        private string banner;
        //private object https;
        //private int multipart;
        //private int form;
        //private int formdata;
        //private string mutipartContent;

        public string Banner
        {
            get { return banner; }
            set
            {
                //if (banner != value)
                //{
                banner = value;
                OnPropertyChanged();
                //};
            }
        }
        public Command UploadImage
        {
            get
            {
                return new Command(async () =>
                    {
                        Banner = await ClickToUpload();
                    });
            }
        }
        public Command SaveImage
        {
            get
            {
                return new Command(async () =>
                {
                   await PostBanner(Banner);

                });
            }
        }
        private async Task<string> ClickToUpload()
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

        //private void UpdateName(string name)
        //{
        //    string url = "https://api.twitter.com/1.1/account/update_profile.json?name=" + name;
        //    PostApi(url);
        //}
        //POST API
        private async Task PostBanner(string ImagePath)
        {
            string mediaId = string.Empty;
            string banner_img = string.Empty;
            try
            {
                Console.WriteLine(ImagePath);
                Authorization auth = new Authorization();
                var url = "https://api.twitter.com/1.1/account/update_profile_banner.json";
                byte[] bannerdata = System.IO.File.ReadAllBytes(ImagePath);
                //string s = Convert.ToBase64String(bannerdata, Base64FormattingOptions.InsertLineBreaks);
                var imgContent = new ByteArrayContent(bannerdata);
                imgContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(imgContent, "banner");

            //    var data = new Dictionary<string, string>
            //{
            //        {"banner",   },
            //        {"width" , "1500"}
            //};

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url, null, "POST" ));

                    var httpResponse = await httpClient.PostAsync(url, multipartContent);
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {

                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }



        private async Task Profile()
        {
            try
            {
                Authorization auth = new Authorization();
                var url = "https://api.twitter.com/1.1/users/show.json";

                using (var httpClient = new HttpClient())
                {
                    var data = new Dictionary<string, string>
                        {
                            { "screen_name", "ashishchopra01" }
                        };
                    //string str = auth.PrepareOAuth(url, null, "GET");
                    httpClient.DefaultRequestHeaders.Add("Authorization", auth.PrepareOAuth(url,data,"GET"));
                    UriBuilder builder = new UriBuilder(url);
                    builder.Query = "screen_name=ashishchopra01";
                    //httpClient.DefaultRequestHeaders.Add());
                    var httpResponse = httpClient.GetAsync(builder.Uri).Result;
                    Console.WriteLine(httpResponse);
                    if (httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.Unauthorized))
                    {
                        DisplayAlert("sorry", "You are not authorized", "ok");
                        return;
                    }
                    var httpContent = await httpResponse.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<ProfilePageModel>(httpContent);
                    Banner = obj.profile_banner_url;
                    ProfileImage = obj.profile_image_url;
                    Name = obj.name;
                    Username = obj.screen_name;
                    Location = obj.location;
                    Description = obj.description;
                }
            }
            catch(Exception ex) { }
        }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }

    }

}
