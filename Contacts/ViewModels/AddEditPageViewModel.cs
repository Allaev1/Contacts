using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Contacts.Services.FileStoringService;
using Contacts.Services.ContactsRepositoryService;
using Contacts.ProxyModels;
using Template10.Mvvm;

namespace Contacts.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService repositoryService;
        IFileStoringService storingService;

        Models.Contacts currentContact;
        ProxyContact _tempContact;
        BitmapImage _image;

        StorageFile imageFile;

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
            storingService = new FileStoringService();
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
            if ((imageFile = await GetImageAsync()) == null) return;

            await storingService.SaveToTempStorageAsync(imageFile, currentContact.ID);

            imageFile = 
                await storingService.GetFileAsync(ApplicationData.Current.TemporaryFolder, currentContact.ID);

            Image = new BitmapImage(new Uri(imageFile.Path));
        }
        #endregion

        #endregion

        #region Private method
        private async Task<StorageFile> GetImageAsync()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            return await picker.PickSingleFileAsync();
        }

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
