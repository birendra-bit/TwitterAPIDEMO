using Newtonsoft.Json;
using Plugin.Media;
using RestSharp;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.ViewModels.Base;
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
        private string banner;
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

        private void UpdateName(string name)
        {
            string url = "https://api.twitter.com/1.1/account/update_profile.json?name=" + name;
            PostApi(url);
        }
        //POST API
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
            request.AddHeader("Authorization", "OAuth oauth_consumer_key=\"Cf1w0izou1SdsMCq7M4wAewlH\",oauth_token=\"1215223960352149504-NI9GmNzFkuwhDO9d1oJ1kbuGDFCSQu\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1579850979\",oauth_nonce=\"fwSUtZy4hPu\",oauth_version=\"1.0\",oauth_signature=\"dW%2Fd14hGxZNtT4ZPEHS1MS6iFzY%3D\"");
            request.AddHeader("Content-Type", "multipart/form-data; boundary=--------------------------487017961943671843106180");
            request.AlwaysMultipartFormData = true;
            IRestResponse response = client.Execute(request);

            obj = JsonConvert.DeserializeObject<ProfilePageModel>(response.Content);
            Banner = obj.profile_banner_url;
            ProfileImage = obj.profile_image_url;
            Name = obj.name;
            Username = obj.screen_name;
            Location = obj.location;
            Description = obj.description;
        }





        public string Name { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }

    }

}
