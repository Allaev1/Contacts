using System;
using System.Collections.Generic;
using System.IO;
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
        #region Declarations
        StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
        #endregion
        public bool HasPhoto { get; set; }
        #region Implementation of IFileStoringService
        public async Task DeleteFromLocalStorageAsync(StorageFile file)
        {
            await file.DeleteAsync();
        }

        public async Task DeleteFromTempStorageAsync(StorageFile file)
        {
            await file.DeleteAsync();
        }

        public async Task<StorageFile> GetFileAsync(StorageFolder parentFolder, string fileName)
        {
            StorageFile expectedFile = await parentFolder.GetFileAsync(fileName);

            return expectedFile;
        }

        public async Task SaveToLocalStorageAsync(StorageFile file, string fileName)
        {
            await file.CopyAsync(ApplicationData.Current.LocalFolder, fileName);
        }

        public async Task SaveToTempStorageAsync(StorageFile fileToSave, string fileName)
        {
            if (await IsFileExist(ApplicationData.Current.TemporaryFolder, fileName))
            {
                StorageFile storageFile = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);

                await storageFile.DeleteAsync();
            }

            await fileToSave.CopyAsync(ApplicationData.Current.TemporaryFolder, fileName);
        }
        #endregion

        #region Private methods
        private async Task<bool> IsFileExist(StorageFolder parentFolder, string fileName)
        {
            if (null == await parentFolder.TryGetItemAsync(fileName))
                return false;

            return true;
        }
        #endregion
    }
}
