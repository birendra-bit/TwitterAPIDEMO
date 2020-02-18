using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIDemo.Models
{
    public class ProfilePageModel
    {
        public string name { get; set; }
        public string screen_name { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string profile_image_url { get; set; }
        public string profile_banner_url { get; set; }
    }
}
