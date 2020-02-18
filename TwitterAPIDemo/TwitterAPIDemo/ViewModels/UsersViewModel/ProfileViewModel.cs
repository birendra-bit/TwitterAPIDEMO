using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Network;
using TwitterAPIDemo.Services;
using TwitterAPIDemo.Utils;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class ProfileViewModel : BaseViewModel
    {
        APIservice _aPIservice;
        ProfilePageModel obj;
        MediaUpload _media;
        MediaContent _mediaContent;
        bool flag = false;
        private string _banner;
        private string _profileImage;
        private string _name;
        private bool _visibility;
        public ProfileViewModel(){}
        public override async Task InitializeAsync(Page page)
        {
            await base.InitializeAsync(page);
            await Profile();
        }
        public bool Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public Command EditProfile
        {
            get
            {
                return new Command(EditUserProfile);
            }
        }
        public Command UploadImage
        {
            get
            {
                return new Command(UploadUserImage);
            }
        }
        public Command SaveImage
        {
            get
            {
                return new Command(SaveUserImage);
            }
        }
        private async void EditUserProfile(object obj)
        {
            string property = obj.ToString();
            Dictionary<string, string> UserData = new Dictionary<string, string>();
            string message = "";
            if (property == "EditName")
            {
                UserData = new Dictionary<string, string>
                    {
                        {"name" , Name }
                    };
                await Update(UserData);
                message = "Name has been updated";
                DisplayFlashingMessage(message);
            }

            else if (property == "EditDescription")
            {
                UserData = new Dictionary<string, string>
                        {
                            {"description", Description }
                        };
                await Update(UserData);
                message = "Description has been updated";
                DisplayFlashingMessage(message);
            }
        }
        public string Banner
        {
            get { return _banner; }
            set
            {
                _banner = value;
                OnPropertyChanged();
            }
        }

        public string ProfileImage
        {
            get { return _profileImage; }
            set
            {
                _profileImage = value;
                OnPropertyChanged();
            }
        }
        private async void UploadUserImage(object obj)
        {
            _media = new MediaUpload();
            string type = obj.ToString();
            if (type == "banner")
            {
                flag = true;
                Banner = await _media.PickPhoto();
                Visibility = true;

            }
            else if (type == "ProfileImage")
            {
                ProfileImage = await _media.PickPhoto();
                Visibility = true;

            }
        }
        private async void SaveUserImage(object obj)
        {
            string save = obj.ToString();
            string message = "";
            if (flag == true)
            {
                var url = "https://api.twitter.com/1.1/account/update_profile_banner.json";
                await UploadProfileMedia(Banner, url.ToString());
                message = "Banner has been Updated";
                flag = false;
            }
            else
            {

                var url = "https://api.twitter.com/1.1/account/update_profile_image.json";
                await UploadProfileMedia(ProfileImage, url.ToString());
                message = "Profile Image has been updated";
            }
            DisplayFlashingMessage(message);
        }
        private async Task Update(Dictionary<string, string> data)
        {
            var url = "https://api.twitter.com/1.1/account/update_profile.json";
            _aPIservice = new APIservice();
            if (await _aPIservice.PostResponse(url, data, null) != null)
            {
                DisplayAlert("Successful", "User Profile updated", "OK");
            }
        }

        private async Task UploadProfileMedia(string ImagePath, string url)
        {
            _aPIservice = new APIservice();
            _mediaContent = new MediaContent();
            MultipartFormDataContent multipartContent;
            if (flag == true)
            {
                multipartContent = _mediaContent.MediaUpload(ImagePath, "banner");
            }
            else
            {
                multipartContent = _mediaContent.MediaUpload(ImagePath, "image");
            }
            if (await _aPIservice.PostResponse(url, null, multipartContent) != null)
            {
                DisplayAlert("Successful", "User Profile updated", "OK");
            }
        }
        private async Task Profile()
        {
            try
            {
                var url = "https://api.twitter.com/1.1/users/show.json";
                _aPIservice = new APIservice();
                var data = new Dictionary<string, string>
                        {
                            { "screen_name", "Birendr19286036" }
                        };
                obj = await _aPIservice.GetResponse<ProfilePageModel>(url, data, "screen_name=Birendr19286036");
                Banner = obj.profile_banner_url;
                ProfileImage = obj.profile_image_url;
                Name = obj.name;
                Username = obj.screen_name;
                Location = obj.location;
                Description = obj.description;

            }
            catch (Exception ex) { }
        }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

    }
}

