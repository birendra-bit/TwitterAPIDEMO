using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterAPIDemo.Services
{
    public class UserDetails
    {
        public UserDetails(){}
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public string ProfileImgUrl { get; set; }
        public string Status { get; set; }
    }
}
