using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIDemo.Models
{
    public class Url
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public List<int> indices { get; set; }
    }
    public class Medium
    {
        public string media_url { get; set; }
        public string media_url_https { get; set; }
    }
    public class Entities
    {
        public List<Url> urls { get; set; }
        public List<Medium> media { get; set; }
    }
    public class User
    {
        //public int id { get; set; }
        //public string id_str { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public string profile_image_url { get; set; }
        public string profile_image_url_https { get; set; }
        public string profile_banner_url { get; set; }
        public bool followed_by { get; set; }
        public bool following { get; set; }
    }

    public class UsersTweets
    {
        public string text { get; set; }
        public User user { get; set; }
        public Entities entities { get; set; }
    }
}
