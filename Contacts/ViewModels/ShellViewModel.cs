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
        #region Fields
        INavigationService navigationService;
        #endregion

        #region Bindable properties
        string _header;
        public string Header
        {
            set { Set(ref _header, value); }
            get { return _header; }
        }
        #endregion

        #region Constructor
        public ShellViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            Header = "Contacts";
        }
        #endregion

        #region Commands

        #region Navigation command
        DelegateCommand<NavigationView> _navigateTo;
        public DelegateCommand<NavigationView> NavigateTo
        {
            get { return _navigateTo ?? new DelegateCommand<NavigationView>(ExecuteNavigateTo); }
        }

        private void ExecuteNavigateTo(NavigationView navigationView)
        {
            NavigationViewItem selectedItem = (NavigationViewItem)navigationView.SelectedItem;

            var tag = selectedItem.Tag;
            string typeString;
            if (tag == null)
                navigationService.Navigate(typeof(SettingsPage));
            else
            {
                typeString = "Contacts.Views." + tag.ToString();
                navigationService.Navigate(Type.GetType(typeString));
                Header = selectedItem.Content.ToString();
            }
        }
        #endregion

        #endregion
    }
}
