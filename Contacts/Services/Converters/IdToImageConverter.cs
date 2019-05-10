using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.Services.Converters
{
    public class IdToImageConverter : IValueConverter
    {
        #region Fields
        //WriteableBitmap contactImage; // фотография контакта id которого конвертируется

        BitmapImage contactImagePlaceHolder = new BitmapImage(new Uri("ms-appx:///Assets/contactImagePlaceHolder.png")); //Фотография контакта если у него нету фотографий в локальном хранилище
        #endregion

        #region Realisation of IValueConverter
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //Контакт фотографию которого нужно показать (на DetailTemplate или ItemTemplate)
            Models.Contacts contactForView = value as Models.Contacts;

            string Id = contactForView.ID;

            StorageFile imageFile = null;

            Task<StorageFile> getImageFileTask = Task.Run(
                async ()
                => imageFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync(Id) as StorageFile);

            getImageFileTask.Wait();

            BitmapImage bitmapImage;

            if (imageFile != null)
            {
                bitmapImage = new BitmapImage(new Uri(imageFile.Path));

                return bitmapImage;
            }
            else
                return new BitmapImage(new Uri("ms-appx:///Assets/contactImagePlaceHolder.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
