using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contacts.Models;

namespace Contacts.Services.ContactsRepositoryService
{
    /// <summary>
    /// Methods that support action above list of contacts and contact itself 
    /// </summary>
    interface IContactRepositoryService
    {
        List<Contact> GetAll();
    }
}
