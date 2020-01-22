using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIDemo.Models
{
    public class Attributes
    {
    }

    public class ProfileLocation
    {
        public string id { get; set; }
        public string url { get; set; }
        public string place_type { get; set; }
        public string name { get; set; }
        public string full_name { get; set; }
        
    }

    public class Description
    {
        public List<object> urls { get; set; }
    }

    public class Entities
    {
        public Description description { get; set; }
    }

    public class Url
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public List<int> indices { get; set; }
    }

    public class Entities2
    {
        public List<object> hashtags { get; set; }
        public List<object> symbols { get; set; }
        public List<object> user_mentions { get; set; }
        public List<Url> urls { get; set; }
    }

    public class ProfilePageModel
    {
        public long id { get; set; }
        public string id_str { get; set; }
        public string name { get; set; }
        public string screen_name { get; set; }
        public string location { get; set; }
        public ProfileLocation profile_location { get; set; }
        public string description { get; set; }
        public object url { get; set; }
        
        public int followers_count { get; set; }
        public int friends_count { get; set; }
        
      
        public object profile_background_image_url_https { get; set; }
        public bool profile_background_tile { get; set; }
        public string profile_image_url { get; set; }
        public string profile_image_url_https { get; set; }
        public string profile_banner_url { get; set; }
       
    }
}
