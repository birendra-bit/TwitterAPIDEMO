using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIDemo.Models
{
    public class Medium
    {
        public string media_url { get; set; }
        public string media_url_https { get; set; }
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
    }

    public class UsersTweets
    {
        public string text { get; set; }
        public User user { get; set; }
    }
}
