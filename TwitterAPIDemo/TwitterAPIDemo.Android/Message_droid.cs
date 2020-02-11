﻿using Android.Widget;
using TwitterAPIDemo.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Message_droid))]
namespace TwitterAPIDemo.Droid
{
    public class Message_droid : iMessage
    {
        public void Longtime(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void Shorttime(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}