using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;
using Windows.UI.Xaml.Controls;
using Contacts.Views.ContentDialogs;

namespace Contacts.Services.DialogService
{
    public class DialogService : IDialogService
    {
        public async Task<ContentDialogResult> ShowDialogAsync(Models.Contacts contactToShow)
        {
            ContactContentDialog contentDialog = new ContactContentDialog();

            contentDialog.ViewModel.CurrentContact = contactToShow;

            return await contentDialog.ShowAsync();
        }
    }
}
