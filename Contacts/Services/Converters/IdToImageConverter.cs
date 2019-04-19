using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.Services.Converters
{
    public class IdToImageConverter : IValueConverter
    {
        #region Realisation of IValueConverter
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //Контакт фотографию которого нужно показать (на DetailTemplate или ItemTemplate)
            Models.Contacts contactForView = value as Models.Contacts;

            string Id = contactForView.ID;

            StorageFile imageFile = null; 

            Task<StorageFile> getImageTask = Task.Run(
                async ()
                => imageFile = await ApplicationData.Current.LocalFolder.TryGetItemAsync(Id) as StorageFile);

            getImageTask.Wait();

            if (imageFile != null)
                return new BitmapImage(new Uri(imageFile.Path));
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
