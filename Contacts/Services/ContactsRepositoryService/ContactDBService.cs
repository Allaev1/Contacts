using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;
using SQLite;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactDBService : IContactRepositoryService
    {
        List<Models.Contacts> _contacts;

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Contacts>> GetAllAsync()
        {
            return _contacts = _contacts ?? await Read();
        }

        #region Read/write
        public async Task<List<Models.Contacts>> Read()
        {
            SQLiteConnection connection = null;
            StorageFile dbFile = null;

            //try
            //{
            //    dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ms-appx:///ContactsDB.db");
            //}
            //catch (Exception e)
            //{
            //    ContentDialog OkCancelDialog = new ContentDialog()
            //    {
            //        Title = $"Exciption",
            //        Content = $"{e.Message}",
            //        PrimaryButtonText = "Ok"
            //    };
            //    ContentDialogResult result = await OkCancelDialog.ShowAsync();
            //}

            if (dbFile == null)
            {
                try
                {
                    //copy db deployed with app
                    //var appDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ContactsDataBase.db"));
                    //dbFile = await appDb.CopyAsync(ApplicationData.Current.LocalFolder);
                    dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDataBase.db");
                    connection = new SQLiteConnection(dbFile.Path);
                }
                catch (Exception e)
                {
                    ContentDialog OkCancelDialog = new ContentDialog()
                    {
                        Title = $"Exciption",
                        Content = $"{e.Message}",
                        PrimaryButtonText = "Ok"
                    };
                    ContentDialogResult result = await OkCancelDialog.ShowAsync();
                }
            }

            await Task.Run(() =>
            {
                _contacts = new List<Models.Contacts>(connection.Table<Models.Contacts>());
            });

            return _contacts;
        }
        #endregion
    }
}
