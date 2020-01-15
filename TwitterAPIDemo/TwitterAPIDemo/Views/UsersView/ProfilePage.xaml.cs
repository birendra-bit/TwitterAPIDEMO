using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterAPIDemo.Views.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TwitterAPIDemo.Views.UsersView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : BaseContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}
	}
}