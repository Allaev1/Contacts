using Template10.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Contacts.ViewModels
{
    public class AddEditDialogPageViewModel : ViewModelBase
    {
        public AddEditDialogPageViewModel()
        {
            NavigationService= WindowWrapper.Current().NavigationServices.FirstOrDefault();
        }

        public DelegateCommand GoBack
        {
            get { return new DelegateCommand(GoBackExecute); }
        }

        private void GoBackExecute() =>
            NavigationService.Navigate(typeof(Views.MasterDetailPage));
    }
}
