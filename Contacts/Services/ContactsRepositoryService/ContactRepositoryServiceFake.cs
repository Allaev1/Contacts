using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;
    
namespace Contacts.Services.ContactsRepositoryService
{
    //Temporary implementation of IContactRepositoryService cause it operates with fake data
    public class ContactRepositoryServiceFake : IContactRepositoryService
    {
        public List<Contact> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
