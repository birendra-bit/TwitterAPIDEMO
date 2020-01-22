using System.Diagnostics;
using TwitterAPIDemo.Views.Base;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersPage : BaseContentPage
    {
        public UsersPage()
        {
            InitializeComponent();
            TapGestureRecognizer_Tapped(null, null);
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            
            content.Content = new FollowingPage();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, System.EventArgs e)
        {
            content.Content = new FollowerPage();
        }
    }
}