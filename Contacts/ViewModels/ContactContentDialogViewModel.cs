using Windows.UI.Xaml.Media.Imaging;
using Contacts.Services.Converters;

namespace Contacts.ViewModels
{
    public class ContactContentDialogViewModel
    {
        #region Fields
        IdToImageConverter converter;
        #endregion

        public ContactContentDialogViewModel()
        {
            converter = new IdToImageConverter();
        }

        #region Bindable properties
        public Models.Contacts CurrentContact
        {
            set; get;
        }

        public bool IsContactFavorite
        {
            set
            {
                if (CurrentContact.IsFavorite == 1 && value == false)
                    CurrentContact.IsFavorite = 0;
                else
                    CurrentContact.IsFavorite = 1;
            }
            get
            {
                if (CurrentContact.IsFavorite == 1)
                    return true;
                else
                    return false;
            }
        }

        public BitmapImage ContactImage
        {
            get { return converter.Convert(CurrentContact, null, null, null) as BitmapImage; }
        }

        public string ContactFullName
        {
            get { return $"{CurrentContact.FirstName} {CurrentContact.LastName}"; }
        }
        #endregion
    }
}
