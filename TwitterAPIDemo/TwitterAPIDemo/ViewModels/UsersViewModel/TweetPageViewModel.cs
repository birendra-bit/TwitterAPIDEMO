using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using TwitterAPIDemo.Models;
using TwitterAPIDemo.Network;
using TwitterAPIDemo.Services;
using TwitterAPIDemo.Utils;
using TwitterAPIDemo.ViewModels.Base;
using Xamarin.Forms;

namespace TwitterAPIDemo.ViewModels.UsersViewModel
{
    public class TweetPageViewModel : BaseViewModel
    {
        string _url = string.Empty;
        APIservice _aPIservice;
        MediaContent _mediaContent;
        private string _text;
        private string _sourceImg;
        MediaUpload _media;
        public TweetPageViewModel()
        {
            _media = new MediaUpload();
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
                    UpdateStatus(Text, SourceImg);
                    Navigation.PopAsync();
                });
            }
        }
        public string SourceImg
        {
            get { return _sourceImg; }
            set
            {
                if (_sourceImg != value)
                {
                    _sourceImg = value;
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
                    SourceImg = await _media.PickPhoto();
                });
            }
        }

        private async void UpdateStatus(string text, string pathToImage)
        {
            try
            {
                if (text == null && pathToImage == null)
                    return;
                string mediaId = string.Empty;
                if (pathToImage != null)
                {
                    _url = "https://upload.twitter.com/1.1/media/upload.json";
                    _aPIservice = new APIservice();
                    _mediaContent = new MediaContent();
                    var mdeiaContent = _mediaContent.MediaUpload(pathToImage, "media");
                    var resp = JsonConvert.DeserializeObject<User>(await _aPIservice.PostResponse(_url, null, mdeiaContent));
                    mediaId = resp.media_id_string;
                }

                _url = "https://api.twitter.com/1.1/statuses/update.json";
                _aPIservice = new APIservice();
                var data = new Dictionary<string, string>{
                    { "status", text },
                    { "trim_user", "1" },
                    { "media_ids", mediaId}
                };
                string response = await _aPIservice.PostResponse(_url, data, null);
                if (response != null)
                {
                    DisplayAlert("Successful", "Your status is uploaded", "ok");
                    return;
                }
            }catch(Exception e) { }
        }
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
