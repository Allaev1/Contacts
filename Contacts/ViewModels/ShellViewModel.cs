using System;
using System.Linq;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Contacts.Views;
using Template10.Common;
using Template10.Mvvm;
using System.Collections.ObjectModel;

namespace Contacts.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        #region Fields
        INavigationService navigationService;
        #endregion

        #region Constructor
        public ShellViewModel()
        {
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            Header = "Contacts";
        }
        #endregion

        #region Bindable properties
        string _header;
        public string Header
        {
            set { Set(ref _header, value); }
            get { return _header; }
        }

        public ObservableCollection<NavigationMenuItem> NavigationMenuItems
        {
            get
            {
                return new ObservableCollection<NavigationMenuItem>()
                {
                    new NavigationMenuItem(){Content="Contacts",Symbol=Symbol.Contact,PageType=typeof(MasterDetailPage)},
                    new NavigationMenuItem(){Content="Favorites",Symbol=Symbol.Favorite,PageType=typeof(FavoritesPage)}
                };
            }
        }

        NavigationMenuItem _selectedItem;
        public NavigationMenuItem SelectedItem
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

    }

    #region Screen object
    /// <summary>
    /// Represent data for menus` item 
    /// </summary>
    public class NavigationMenuItem
    {
        public object Content { set; get; }
        /// <summary>
        /// Symbol that shown next to content
        /// </summary>
        public Symbol Symbol { set; get; }
        /// <summary>
        /// Page to that make navigation after item was tapped
        /// </summary>
        public Type PageType { set; get; }
    }
    #endregion
}
