using Contacts.ProxyModels;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Services.FileStoringService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using GalaSoft.MvvmLight.Messaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Contacts.Message;
using static Template10.Common.BootStrapper;

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
        WriteableBitmap _writeableImage;
        bool _isEnabled;
        string _pageHeader;

        StorageFile imageFile;

        DelegateCommand _goBackSaved;
        DelegateCommand _goBackUnsaved;
        DelegateCommand _addImage;
        DelegateCommand _removeImage;

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

        public string PageHeader { set; get; }

        public BitmapImage Image
        {
            set
            {
                Set(ref _image, value);
                RemoveImage.RaiseCanExecuteChanged();
            }
            get { return _image; }
        }

        public WriteableBitmap WriteableBitmap
        {
            set { Set(ref _writeableImage, value); }
            get { return _writeableImage; }
        }

        public bool IsEnabled { private set { Set(ref _isEnabled, value); } get { return _isEnabled; } }

        #endregion

        #region Constructors
        public AddEditPageViewModel(IFileStoringService fileStoringService, IContactRepositoryService contactRepositoryService)
        {
            repositoryService = contactRepositoryService;
            storingService = fileStoringService;
            _goBackSaved = new DelegateCommand(GoBackSavedExecute);
            _goBackUnsaved = new DelegateCommand(GoBackUnsavedExecute);
            _removeImage = new DelegateCommand(RemoveImageExecute, CanRemoveImageExecute);
            IsEnabled = false;
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (isDone
                || parameter == null && currentState == States.Edit
                || parameter != null && currentState == States.Add)
            {
                isDone = false; //Для каждой новой операций флаг опускается
                SetTempPerson(parameter);

                if (TempContact.PathToImage != null)
                    Image = new BitmapImage(new Uri(currentContact.PathToImage));
                else
                    Image = new BitmapImage(new Uri("ms-appx:///Assets/contactImagePlaceHolder.png"));
            }

            if (parameter == null)
            {
                PageHeader = "Adding new person";
                currentState = States.Add;
            }
            else
            {
                PageHeader = $"Editing {TempContact.FullName}";
                currentState = States.Edit;
            }

            Messenger.Default.Register<IsDirtyMessage>(this, (message) => HandleIsDirtyChangedMessage(message));

            await Task.CompletedTask;
        }

        public async override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (isDone)
            {
                imageFile = null;
                await ApplicationData.Current.ClearAsync(ApplicationDataLocality.Temporary);
            }
            App.Current.ShowShellBackButton = true;
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
            await SetCurrentContactAsync();

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

            await storingService.SaveToStorage(ApplicationData.Current.TemporaryFolder, imageFile, imageFile.Name);

            imageFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(imageFile.Name);

            TempContact.PathToImage = imageFile.Path;

            Image = new BitmapImage(new Uri(imageFile.Path));
        }
        #endregion

        #region Remove Image
        public DelegateCommand RemoveImage
        {
            get { return _removeImage ?? new DelegateCommand(RemoveImageExecute, CanRemoveImageExecute); }
        }

        private bool CanRemoveImageExecute()
        {
            if (Image == null) return false;

            else if (TempContact.PathToImage == null) return false;

            else if (TempContact.PathToImage.Equals(Image.UriSource.LocalPath)) return true;

            else return false;
        }

        private void RemoveImageExecute()
        {
            TempContact.PathToImage = null;

            Image = null;
        }
        #endregion

        #endregion

        #region Private methods
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

        private async Task SetCurrentContactAsync()
        {
            currentContact.FirstName = TempContact.FirstName;
            currentContact.LastName = TempContact.LastName;
            currentContact.IsFavorite = TempContact.IsFavorite;
            currentContact.PhoneNumber = TempContact.PhoneNumber;
            currentContact.Email = TempContact.Email;

            await SetImageContactAsync();
        }

        private async Task SetImageContactAsync()
        {
            if (currentContact.PathToImage == null)
            {
                if (imageFile == null) return;

                await storingService.SaveToStorage(ApplicationData.Current.LocalFolder, imageFile, currentContact.ID);

                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(currentContact.ID);

                currentContact.PathToImage = file.Path;
            }
            else
            {
                if (TempContact.PathToImage == null)
                {
                    StorageFile fileToDelete =
                        await StorageFile.GetFileFromPathAsync(currentContact.PathToImage);

                    await fileToDelete.DeleteAsync();

                    currentContact.PathToImage = null;
                }
                else
                {
                    if (imageFile == null) return;

                    await storingService.SaveToStorage(ApplicationData.Current.LocalFolder, imageFile, currentContact.ID);

                    StorageFile newImageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(currentContact.ID);

                    currentContact.PathToImage = newImageFile.Path;
                }
            }
        }
        #endregion

        #region Message handler
        private void HandleIsDirtyChangedMessage(IsDirtyMessage message)
        {
            bool isDirty = message.IsDirty;

            if (TempContact.IsValid && isDirty) IsEnabled = true;
            else IsEnabled = false;
        }
        #endregion
    }
}
