using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Contacts.ViewModels
{
    class AddEditPageViewModel : ViewModelBase
    {
        #region Fields

        #endregion

        #region Constructors
        public AddEditPageViewModel()
        {

        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return base.OnNavigatedToAsync(parameter, mode, state);
        }
        #endregion
    }
}
