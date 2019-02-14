using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Contacts.Services.ContactsRepositoryService;

namespace Contacts.ViewModels
{
    class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService repositoryService;
        #endregion

        #region Constructors
        public AddEditPageViewModel()
        {
            repositoryService = new ContactDBService();
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {


            return Task.CompletedTask;
        }
        #endregion
    }
}
