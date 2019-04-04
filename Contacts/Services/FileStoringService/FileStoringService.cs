using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Contacts.Services.FileStoringService
{
    /// <summary>
    /// Делает файловые операций
    /// </summary>
    public class FileStoringService : IFileStoringService
    {
        public async Task DeleteFromLocalStorageAsync(StorageFile file)
        {
            await file.DeleteAsync();
        }

        public async Task DeleteFromTempStorageAsync(StorageFile file)
        {
            await file.DeleteAsync();
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder folder, string fileName)
        {
            StorageFile expectedFile = await folder.GetFileAsync(fileName);

            return expectedFile;
        }

        public async Task SaveToLocalStorageAsync(StorageFile file, string fileName)
        {
            await file.CopyAsync(ApplicationData.Current.LocalFolder, fileName);
        }

        public async Task SaveToTempStorageAsync(StorageFile file, string fileName)
        {
            await file.CopyAsync(ApplicationData.Current.TemporaryFolder, fileName);
        }
    }
}
