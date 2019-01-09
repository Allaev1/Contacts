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
    public interface IContactRepositoryService
    {
        Task<List<Models.Contacts>> GetAllAsync();

        Task DeleteAsync(string id);
    }
}
