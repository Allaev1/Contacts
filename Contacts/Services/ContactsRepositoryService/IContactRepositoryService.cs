using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contacts.Services.ContactsRepositoryService
{
    /// <summary>
    /// Methods that support action above list of contacts and contact itself 
    /// </summary>
    public interface IContactRepositoryService
    {
        Task<List<Models.Contacts>> GetAllAsync();

        Task AddAsync(Models.Contacts contact);

        Task DeleteAsync(string id);

        Task UpdateAsync(Models.Contacts contact);

        Task<List<Models.Contacts>> GetAllFavoritesAsync();

        Task<Models.Contacts> GetByIdAsync(string id);
    }
}
