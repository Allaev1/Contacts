using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;

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
            return _contacts = _contacts ?? await ReadAsync();
        }

        #region Read/write
        public async Task<List<Models.Contacts>> ReadAsync()
        {
            StorageFile dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDatabase.db");
            SQLiteConnection connection = new SQLiteConnection(dbFile.Path);

            return await Task.Run(() =>
            {
               return _contacts = new List<Models.Contacts>(connection.Table<Models.Contacts>());
            });
        }
        #endregion
    }
}
