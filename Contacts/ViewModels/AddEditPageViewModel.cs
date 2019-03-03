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

namespace Contacts.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        IContactRepositoryService repositoryService;
        Models.Contacts currentContact;
        ProxyContact _temporaryContact;
        DelegateCommand _goBackSaved;
        DelegateCommand _goBackUnsaved;
        DelegateCommand _addImage;
        object _image;
        StorageFile ImageBytes;
        byte[] imageBytes = null;
        #endregion

        #region Bindable properties
        public ProxyContact TemporaryContact
        {
            set { Set(ref _temporaryContact, value); }
            get { return _temporaryContact; }
        }

        public object Image
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
            _addImage = new DelegateCommand(AddImageExecute);
        }
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            currentContact = parameter == null ? new Models.Contacts() { ID = Guid.NewGuid().ToString() } : null;

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
            TemporaryContact = temporary;
            TemporaryContact.Validate();

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
            currentContact.FirstName = TemporaryContact.FirstName;
            currentContact.LastName = TemporaryContact.LastName;
            currentContact.IsFavorite = TemporaryContact.IsFavorite;
            currentContact.PhoneNumber = TemporaryContact.PhoneNumber;
            currentContact.Email = TemporaryContact.Email;

            await FileIO.WriteBytesAsync(ImageBytes, imageBytes); //Записываем массив байтов фотографий в изолированное хранилище

            await repositoryService.AddAsync(currentContact);

            NavigationService.Navigate(typeof(Views.MasterDetailPage));
        }
        #endregion

        #region Unsave
        public DelegateCommand GoBackUnSaved
        {
            get { return _goBackUnsaved ?? new DelegateCommand(GoBackSavedExecute); }
        }

        public async void GoBackUnsavedExecute() =>
            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        #endregion

        #region Pick Image
        public DelegateCommand AddImage
        {
            get { return _addImage ?? new DelegateCommand(AddImageExecute); }
        }

        private async void AddImageExecute()
        {
            //Настройка FileOpenPicker
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpeg");

            StorageFile image = await picker.PickSingleFileAsync(); // Выбранная фотография
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder; //Локальное хранилище

            if (image != null)
            {
                //конвертирование файла в массив байтов
                using (var stream = await image.OpenReadAsync())
                {
                    imageBytes = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(imageBytes);
                    }
                }

                try
                {
                    ImageBytes = await storageFolder.CreateFileAsync(currentContact.ID);
                }
                catch (Exception)
                {
                    var file = await storageFolder.GetFileAsync(currentContact.ID);
                    await file.DeleteAsync();

                    ImageBytes = await storageFolder.CreateFileAsync(currentContact.ID);
                }

                //StorageFile storageFile = await storageFolder.GetFileAsync(currentContact.ID); //Проверка наличия фаила в локальном хранилище

                //конвертирование массива байтов в фотографию
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(imageBytes);
                        await writer.StoreAsync();
                    }
                    var convertedImage = new BitmapImage();
                    await convertedImage.SetSourceAsync(stream);
                    Image = convertedImage;
                }

            }
        }

        #endregion

        #endregion
    }
}
