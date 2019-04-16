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
    public class ToImageConverter : IValueConverter
    {
        #region Realisation of IValueConverter
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string contactId = value.ToString();

            //StorageFile imageFile = await ApplicationData.Current.LocalFolder.GetItemAsync(contactId).GetResults() as StorageFile;

            //return new BitmapImage(new Uri(imageFile.Path));
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
