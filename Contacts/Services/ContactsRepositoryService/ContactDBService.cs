using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;
using Contacts.Message;
using GalaSoft.MvvmLight.Messaging;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactDBService : IContactRepositoryService
    {
        #region Fields
        List<Models.Contacts> _contacts;
        StorageFile dbFile;
        SQLiteConnection connection;
        #endregion

        #region Interface implementation
        public async Task DeleteAsync(string id)
        {
            Models.Contacts SelectedContact = _contacts.Find(a => a.ID == id);
            connection.Table<Models.Contacts>().Delete(a => a.ID == id);
            _contacts.Remove(SelectedContact);

            var message = new OperationResultMessage(){ Operation = CRUD.Delete };
            Messenger.Default.Send<OperationResultMessage>(message);

            await Task.CompletedTask;
        }

        public async Task<List<Models.Contacts>> GetAllAsync()
        {
            if (_contacts == null)
            {
                dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDatabase.db");
                connection = new SQLiteConnection(dbFile.Path);
                return _contacts = await ReadAsync();
            }
            else
                return _contacts;
        }
        #endregion

        #region Read
        public async Task<List<Models.Contacts>> ReadAsync()
        {
            return await Task.Run(() =>
            {
                return _contacts = new List<Models.Contacts>(connection.Table<Models.Contacts>());
            });
        }
        #endregion
    }
}
