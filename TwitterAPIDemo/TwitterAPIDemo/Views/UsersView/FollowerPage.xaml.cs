using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterAPIDemo.ViewModels.UsersViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FollowerPage : ContentView
	{
		public FollowerPage ()
		{
			InitializeComponent ();
            BindingContext = new FollowerViewModel();
		}
	}
}