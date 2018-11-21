using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Template10.Mvvm;

namespace Contacts.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        #region Events handlers
        public void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationViewItem selecteItem = args.SelectedItem as NavigationViewItem;
            switch (selecteItem.Tag)
            {
                case "Contacts":
                    NavigationService.Navigate(typeof(MasterDetailPage));
                    break;
                default:
                    throw new Exception();
            }
        }
        #endregion
    }
}
