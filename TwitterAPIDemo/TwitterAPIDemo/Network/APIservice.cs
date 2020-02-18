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

        //public async Task<string> GetResponse(string url, Dictionary<string,string> data, string query)
        //{
        //    using (_httpClient) {
        //        _httpClient.DefaultRequestHeaders.Add("Authorization", _auth.PrepareOAuth(url, data, "GET"));
        //        UriBuilder builder = new UriBuilder(url);
        //        if (query != "")
        //        {
        //            builder.Query = query;
        //        }
        //        var httpResponse = _httpClient.GetAsync(builder.Uri).Result;

        //        if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
        //        {
        //            DisplayAlert("sorry", "You are not authorized", "ok");
        //            return null;
        //        }
        //        return (await httpResponse.Content.ReadAsStringAsync()).ToString();
        //    }

        //}
        public async Task<T> GetResponse<T>(string url, Dictionary<string, string> data, string query)
        {
            using (_httpClient)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", _auth.PrepareOAuth(url, data, "GET"));
                UriBuilder builder = new UriBuilder(url);
                if (query != "")
                {
                    builder.Query = query;
                }
                var httpResponse = _httpClient.GetAsync(builder.Uri).Result;

                if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    DisplayAlert("sorry", "You are not authorized", "ok");
                    return default(T);
                }
                var response = JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
                return response;
            }
        }
            public async Task<string> PostResponse( string url, Dictionary<string, string> data, MultipartFormDataContent multipartContent)
        {
            using (_httpClient)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", _auth.PrepareOAuth(url, data, "POST"));

                    if (multipartContent != null)
                    {
                        var mediaHttpResponse = await _httpClient.PostAsync(url, multipartContent);

                        if (!mediaHttpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                        {
                            DisplayAlert("sorry", "You are not authorized", "ok");
                        return null;
                        }
                    return (await mediaHttpResponse.Content.ReadAsStringAsync());
                }

                var httpResponse = await _httpClient.PostAsync(url, new FormUrlEncodedContent(data));

                if (!httpResponse.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                {
                    DisplayAlert("sorry", "You are not authorized", "ok");
                    return null;
                }
                return (await httpResponse.Content.ReadAsStringAsync());
            }
        }

    }
}
