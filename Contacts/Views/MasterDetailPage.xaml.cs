using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Contacts.ViewModels;
using System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Contacts.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MasterDetailPage : Page
    {
        public MasterDetailPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            DataContext = new MasterDetailPageViewModel();
        }

        MasterDetailPageViewModel _viewModel;
        public MasterDetailPageViewModel ViewModel
        {
            get { return _viewModel ?? (_viewModel = (MasterDetailPageViewModel)DataContext); }
        }
    }
}
