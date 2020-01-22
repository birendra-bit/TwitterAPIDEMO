using TwitterAPIDemo.ViewModels.UsersViewModel;
using TwitterAPIDemo.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TweetPage : BaseContentPage
    {
        public TweetPage()
        {
            InitializeComponent();
            BindingContext = new TweetPageViewModel(Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}