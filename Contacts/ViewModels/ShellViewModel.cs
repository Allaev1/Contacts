using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template10.Services.NavigationService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Contacts.Views;
using Template10.Common;
using Template10.Mvvm;

namespace Contacts.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        INavigationService navigationService;
        public ShellViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
        }

        public void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)args.SelectedItem;

            var tag = selectedItem.Tag;

            switch (selectedItem.Tag)
            {
                case "MasterDetailPage":
                    navigationService.Navigate(typeof(MasterDetailPage));
                    break;
            }
        }
    }
}
