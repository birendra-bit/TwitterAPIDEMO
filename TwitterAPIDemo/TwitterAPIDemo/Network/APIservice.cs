using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TwitterAPIDemo.ViewModels.Base;

namespace TwitterAPIDemo.Network
{
    public class APIservice:BaseViewModel
    {
        HttpClient _httpClient;
        Oauth _auth;
        public APIservice()
        {
            _httpClient = new HttpClient();
            _auth = new Oauth();
        }
        
        public async Task<string> GetResponse(string url, Dictionary<string,string> data)
        {
            using (_httpClient) {
                _httpClient.DefaultRequestHeaders.Add("Authorization", _auth.PrepareOAuth(url, data, "GET"));
                UriBuilder builder = new UriBuilder(url);

                var httpResponse = _httpClient.GetAsync(builder.Uri).Result;

                if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    DisplayAlert("sorry", "You are not authorized", "ok");
                    return null;
                }
                return (await httpResponse.Content.ReadAsStringAsync()).ToString();
            }
        }
        public async Task<string> PostResponse( string url, Dictionary<string, string> data, string pathToImage)
        {
            using (_httpClient)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", _auth.PrepareOAuth(url, data, "POST"));

                    if (pathToImage != null)
                    {
                        byte[] imgdata = System.IO.File.ReadAllBytes(pathToImage);

                        var imageContent = new ByteArrayContent(imgdata);
                            imageContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");

                        var multipartContent = new MultipartFormDataContent();
                            multipartContent.Add(imageContent, "media");

                         var mediaHttpResponse = await _httpClient.PostAsync(url, multipartContent);

                        if (!mediaHttpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                        {
                            DisplayAlert("sorry", "You are not authorized", "ok");
                            return null;
                        }
                        return (await mediaHttpResponse.Content.ReadAsStringAsync()).ToString();
                    }

                var httpResponse = await _httpClient.PostAsync(url, new FormUrlEncodedContent(data));

                if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    DisplayAlert("sorry", "You are not authorized", "ok");
                    return null;
                }
                return (await httpResponse.Content.ReadAsStringAsync()).ToString();
            }
        }
    }
}
