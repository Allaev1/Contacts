using Contacts.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Contacts.Views.ContentDialogs
{
    public sealed partial class ContactContentDialog : ContentDialog
    {
        public ContactContentDialog()
        {
            this.InitializeComponent();
            ViewModel = SimpleIoc.Default.GetInstance<ContactContentDialogViewModel>();
        }

        public ContactContentDialogViewModel ViewModel { set; get; }
    }
}
