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
	public partial class SearchUserPage : ContentPage
	{
		public SearchUserPage ()
		{
			InitializeComponent ();
            BindingContext = new SearchViewModel();
		}

        //public object MainSearchBar { get; private set; }

        //private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        //{
        //    var keyword = MainSearchBar.Text;
        //}
    }
}