using Windows.UI.Xaml.Media.Imaging;
using Contacts.Services.Converters;
using Contacts.Services.ContactsRepositoryService;
using System.Threading.Tasks;

namespace Contacts.ViewModels
{
    public class ContactContentDialogViewModel
    {
        #region Fields
        IdToImageConverter converter;
        IContactRepositoryService repositoryService;
        #endregion

        public ContactContentDialogViewModel(IContactRepositoryService repositoryService)
        {
            converter = new IdToImageConverter();
            this.repositoryService = repositoryService;
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

                Task.Run(async () => await repositoryService.UpdateAsync(CurrentContact));
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
