using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;

namespace Contacts.Services.ContactsRepositoryService
{
    class ContactDBService : IContactRepositoryService
    {
        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Contact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
