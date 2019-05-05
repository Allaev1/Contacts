using Contacts.Message;
using GalaSoft.MvvmLight.Messaging;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactRepositoryService : IContactRepositoryService
    {
        #region Fields
        List<Models.Contacts> _contacts;
        SQLiteConnection connection;
        #endregion

        #region Contstructors
        public ContactRepositoryService()
        {
            var app = Application.Current as App;
            connection = app.DataBase;
        }
        #endregion

        #region Interface implementation
        public async Task AddAsync(Models.Contacts contact)
        {
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

            var message = new OperationResultMessage() { Operation = CRUD.Update };
            Messenger.Default.Send<OperationResultMessage>(message);

            await Task.CompletedTask;
        }

        public async Task<List<Models.Contacts>> GetAllFavoritesAsync()
        {
            return await Task.Run(() =>
            {
                List<Models.Contacts> favoriteContacts = (from contact in _contacts
                                                          where contact.IsFavorite == 1
                                                          select contact).ToList();
                return favoriteContacts;
            });
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
