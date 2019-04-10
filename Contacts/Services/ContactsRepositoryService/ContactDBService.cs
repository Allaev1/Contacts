using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;
using Contacts.Message;
using GalaSoft.MvvmLight.Messaging;
using Contacts.Models;
using System.Linq;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactDBService : IContactRepositoryService
    {
        #region Fields
        List<Models.Contacts> _contacts;
        StorageFile dbFile;
        SQLiteConnection connection;
        #endregion

        #region Contstructors
        public ContactDBService()
        {

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

        public async Task UpdateAsync(Models.Contacts contact)
        {
            string editedContactId = contact.ID;

            connection.Table<Models.Contacts>().Delete(a => a.ID == editedContactId);

            _contacts.Remove(_contacts.FirstOrDefault(a => a.ID == editedContactId));
            _contacts.Add(contact);

            connection.Table<Models.Contacts>().Connection.Insert(contact);

            var message = new OperationResultMessage() { Operation = CRUD.Edit };
            Messenger.Default.Send<OperationResultMessage>(message);

            await Task.CompletedTask;
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
