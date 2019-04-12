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
            StorageFile stubFile;
            StorageFile expectedFile;

            if (await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) != null)
                return expectedFile = await parentFolder.GetFileAsync(fileName);
            else if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName) == null)
                return stubFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("stub");
            else
                return expectedFile = await parentFolder.GetFileAsync(fileName);
        }

        public async Task SaveToLocalStorageAsync(StorageFile fileToSave, string fileName)
        {
            if (await IsFileExist(ApplicationData.Current.LocalFolder, fileName))
            {
                StorageFile fileToDelete = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                await fileToDelete.DeleteAsync();

                await fileToSave.CopyAsync(ApplicationData.Current.LocalFolder, fileName);
            }
            else
            {
                StorageFile fileToMove= await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileToSave.Name); 

                await fileToMove.MoveAsync(ApplicationData.Current.LocalFolder, fileName);
            }

        }

        public async Task SaveToTempStorageAsync(StorageFile fileToSave, string fileName)
        {
            if (await IsFileExist(ApplicationData.Current.TemporaryFolder, fileName))
            {
                StorageFile fileToDelete = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);

                await fileToDelete.DeleteAsync();
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
