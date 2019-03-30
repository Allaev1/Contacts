using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;
using Contacts.Message;
using GalaSoft.MvvmLight.Messaging;
using Contacts.Models;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactDBService : IContactRepositoryService
    {
        #region Fields
        List<Models.Contacts> _contacts;
        StorageFile dbFile;
        SQLiteConnection connection;
        static ContactDBService _instance = new ContactDBService();
        #endregion

        #region Contstructors
        private ContactDBService()
        {
            
        }
        #endregion

        #region Properties
        public static ContactDBService Instance
        {
            get { return _instance; }
        }
        #endregion

        #region Interface implementation
        public async Task AddAsync(Models.Contacts contact)
        {
            dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDB.db");
            connection = new SQLiteConnection(dbFile.Path);

            connection.Table<Models.Contacts>().Connection.Insert(contact);

            _contacts = await ReadAsync();
        }

        public async Task DeleteAsync(string id)
        {
            Models.Contacts SelectedContact = _contacts.Find(a => a.ID == id);
            connection.Table<Models.Contacts>().Delete(a => a.ID == id);
            _contacts.Remove(SelectedContact);

            var message = new OperationResultMessage() { Operation = CRUD.Delete };
            Messenger.Default.Send<OperationResultMessage>(message);

            await Task.CompletedTask;
        }

        public async Task<List<Models.Contacts>> GetAllAsync()
        {
            if (_contacts == null)
            {
                dbFile = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDB.db");
                connection = new SQLiteConnection(dbFile.Path);
                return _contacts = await ReadAsync();
            }
            else
                return _contacts;
        }

        public Task UpdateAsync(Models.Contacts contact)
        {
            throw new NotImplementedException();
        }

        public Task<List<Models.Contacts>> GetAllFavoritesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Models.Contacts> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
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
