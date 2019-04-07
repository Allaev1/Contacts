using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Contacts.Services.ContactsRepositoryService;
using Contacts.ProxyModels;
using Contacts.Models;
using Windows.Storage.Pickers;
using Windows.ApplicationModel.Contacts;
using System.IO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;
using System.Drawing;
using Windows.UI.Xaml.Controls;
using System.Runtime.InteropServices.WindowsRuntime;
using Template10.Services.NavigationService;

namespace Contacts.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService repositoryService;

        Models.Contacts currentContact;
        ProxyContact _tempContact;
        BitmapImage _image;

        DelegateCommand _goBackSaved;
        DelegateCommand _goBackUnsaved;
        DelegateCommand _addImage;

        bool isDone = true; // Показывает закончен ли процесс добавления или редактирование 

        enum States { Edit, Add };
        States currentState;
        #endregion`

        #region Bindable properties
        public ProxyContact TempContact
        {
            set { Set(ref _tempContact, value); }
            get { return _tempContact; }
        }

        public BitmapImage Image
        {
            set { Set(ref _image, value); }
            get { return _image; }
        }
        #endregion

        #region Constructors
        public AddEditPageViewModel()
        {
            repositoryService = ContactDBService.Instance;
            _goBackSaved = new DelegateCommand(GoBackSavedExecute);
            _goBackUnsaved = new DelegateCommand(GoBackUnsavedExecute);
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (isDone
                || parameter == null && currentState == States.Edit
                || parameter != null && currentState == States.Add)
            {
                isDone = false; //Для каждой новой операций флаг опускается
                SetTempPerson(parameter);
            }

            if (parameter == null)
                currentState = States.Add;
            else
                currentState = States.Edit;

            return Task.CompletedTask;
        }
        #endregion

        #region Commands

        #region Save contact
        public DelegateCommand GoBackSaved
        {
            get { return _goBackSaved ?? new DelegateCommand(GoBackSavedExecute); }
        }

        public async void GoBackSavedExecute()
        {
            currentContact.FirstName = TempContact.FirstName;
            currentContact.LastName = TempContact.LastName;
            currentContact.IsFavorite = TempContact.IsFavorite;
            currentContact.PhoneNumber = TempContact.PhoneNumber;
            currentContact.Email = TempContact.Email;

            if (currentState == States.Add)
                await repositoryService.AddAsync(currentContact);
            else if (currentState == States.Edit)
                await repositoryService.UpdateAsync(currentContact);

            isDone = true;

            NavigationService.Navigate(typeof(Views.MasterDetailPage));
        }
        #endregion

        #region Unsave
        public DelegateCommand GoBackUnSaved
        {
            get { return _goBackUnsaved ?? new DelegateCommand(GoBackSavedExecute); }
        }

        public async void GoBackUnsavedExecute()
        {
            isDone = true;
            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }
        #endregion

        #region Pick Image
        public DelegateCommand AddImage
        {
            get { return _addImage ?? new DelegateCommand(AddImageExecute); }
        }

        private async void AddImageExecute()
        {
            await Task.CompletedTask;
        }
        #endregion

        #endregion

        #region Private method
        private void SetTempPerson(object contact)
        {
            currentContact = contact == null ?
                new Models.Contacts() { ID = Guid.NewGuid().ToString() } : contact as Models.Contacts;

            //TODO: Можно ли вместо создания нового прокси-контакта
            //присвоить занчение полю TemporaryContact (Проверить)
            var temporary = new ProxyContact(currentContact)
            {
                FirstName = currentContact.FirstName,
                LastName = currentContact.LastName,
                IsFavorite = currentContact.IsFavorite,
                Email = currentContact.Email,
                PhoneNumber = currentContact.PhoneNumber,
                GroupID = currentContact.GroupID,
                Validator = i =>
                {
                    var u = i as ProxyContact;
                    if (string.IsNullOrEmpty(u.FirstName))
                        u.Properties[nameof(u.FirstName)].Errors.Add("Firstname is required");
                    if (string.IsNullOrEmpty(u.LastName))
                        u.Properties[nameof(u.LastName)].Errors.Add("Lastname is required");
                    else if (u.LastName.Length < 3)
                        u.Properties[nameof(u.LastName)].Errors.Add("Lastname must consist of minimum 3 characters");
                },
            };
            TempContact = temporary;
            TempContact.Validate();
        }
        #endregion
    }
}
