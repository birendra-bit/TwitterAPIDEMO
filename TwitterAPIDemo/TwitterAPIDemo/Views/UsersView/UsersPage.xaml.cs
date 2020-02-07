using TwitterAPIDemo.ViewModels.UsersViewModel;
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
            BindingContext = new UserPageViewModel();
            //DisplayFollowingUsers(null, null);
        }

        //private void DisplayFollowingUsers(object sender, System.EventArgs e)
        //{

        //    content.Content = new FollowingPage();
        //    following.BackgroundColor = Color.FromHex("#00bfff");
        //    follower.BackgroundColor = Color.Transparent;
        //}

        //private void DisplayFollower(object sender, System.EventArgs e)
        //{
        //    content.Content = new FollowerPage();
        //    following.BackgroundColor = Color.Transparent;
        //    follower.BackgroundColor = Color.FromHex("#00bfff");
        //}

        //private void Button_Clicked(object sender, System.EventArgs e)
        //{
        //    Navigation.PushAsync(new SearchUserPage());
        //}
    }
}