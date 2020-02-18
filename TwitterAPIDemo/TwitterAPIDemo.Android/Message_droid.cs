using Android.Widget;
using TwitterAPIDemo.Droid;
using TwitterAPIDemo.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(Message_droid))]
namespace TwitterAPIDemo.Droid
{
    public class Message_droid : IPlatformHelperService
    {
        public void ShowFlashingMessage(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}