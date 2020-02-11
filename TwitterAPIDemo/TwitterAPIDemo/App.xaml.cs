using TwitterAPIDemo.Views.UsersView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TwitterAPIDemo
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                #if DEBUG
                LiveReload.Init();
                #endif
                InitializeComponent();
                MainPage = new NavigationPage(new TabbedPageContainer());

            }
            catch (System.Exception ex)
            {

            }
            
            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
