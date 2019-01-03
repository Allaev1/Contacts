using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template10.Services.NavigationService;
using System.Threading.Tasks;
using Contacts.Models;
using Windows.UI.Xaml.Controls;
using Contacts.Views;
using Template10.Common;
using Template10.Mvvm;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Command;
using System.Runtime.CompilerServices;
using System.ComponentModel;

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

        public ObservableCollection<MenuItem> Items
        {
            get
            {
                return new ObservableCollection<MenuItem>()
                {
                    new MenuItem(){Content="Contacts",Symbol=Symbol.Contact,PageType=typeof(MasterDetailPage)},
                    new MenuItem(){Content="Favorites",Symbol=Symbol.Favorite,PageType=typeof(FavoritesPage)}
                };
            }
        }

        MenuItem _selectedItem;
        public MenuItem SelectedItem
        {
            set
            {
                Set(ref _selectedItem, value);
                Type pageType = SelectedItem.PageType;
                navigationService.Navigate(pageType);
            }
            get { return _selectedItem; }
        }
        #endregion

        #region Constructor
        public ShellViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            _navigateTo = new DelegateCommand<object>(ExecuteNavigateTo);
            Header = "Contacts";
        }
        #endregion

        #region Commands

        #region Navigation command
        DelegateCommand<object> _navigateTo;
        public DelegateCommand<object> NavigateTo
        {
            get { return _navigateTo ?? new DelegateCommand<object>(ExecuteNavigateTo); }
        }

        private void ExecuteNavigateTo(object item)
        {
            Type pageType = SelectedItem.PageType;

            if (pageType == null)
                navigationService.Navigate(typeof(SettingsPage));
            else
            {
                navigationService.Navigate(pageType);
                Header = SelectedItem.Content.ToString();
            }
        }
        #endregion

        #endregion
    }
}
