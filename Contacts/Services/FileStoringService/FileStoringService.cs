using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Foundation;

namespace Contacts.Services.FileStoringService
{
    /// <summary>
    /// Делает файловые операций
    /// </summary>
    public class FileStoringService : IFileStoringService
    {
        public async Task SaveToStorage(StorageFolder parentFolder, StorageFile file, string fileName)
        {
            if (parentFolder.Name == "LocalState")
                await ToLocalStorageAsync(file, fileName);
            else if (parentFolder.Name == "TempState")
                await ToTempStorageAsync(file, fileName);
        }

        #region Private methods
        /// <summary>
        /// Сохраняет файл в локальное хранилище
        /// </summary>
        /// <param name="fileToSave">
        /// Файл который нужно сохранить
        /// </param>
        /// <param name="fileName">
        /// Имя которое присваевается файлу при сохранение
        /// </param>
        private async Task ToLocalStorageAsync(StorageFile fileToSave, string fileName)
        {
            if (await IsFileExist(ApplicationData.Current.LocalFolder, fileName))
            {
                StorageFile fileToReplace = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                await fileToSave.CopyAndReplaceAsync(fileToReplace);
            }
            else
            {
                StorageFile fileToMove = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileToSave.Name);

                await fileToMove.MoveAsync(ApplicationData.Current.LocalFolder, fileName);
            }
        }

        /// <summary>
        /// Сохраняет файл во временное хранилище
        /// </summary>
        /// <param name="fileToSave">
        /// Файл который нужно сохранить
        /// </param>
        /// <param name="fileName">
        /// Имя которое присваевается файлу при сохранение
        /// </param>
        private async Task ToTempStorageAsync(StorageFile fileToSave, string fileName)
        {
            //if (await IsFileExist(ApplicationData.Current.TemporaryFolder, fileName))
            //{
            //    StorageFile fileToDelete = await ApplicationData.Current.TemporaryFolder.GetFileAsync(fileName);

            //    await fileToSave.CopyAsync(ApplicationData.Current.TemporaryFolder, fileName, NameCollisionOption.ReplaceExisting);
            //}

            //await fileToSave.CopyAsync(ApplicationData.Current.TemporaryFolder, fileName);

            await fileToSave.CopyAsync(ApplicationData.Current.TemporaryFolder, fileName, NameCollisionOption.ReplaceExisting);
        }

        private async Task<bool> IsFileExist(StorageFolder parentFolder, string fileName)
        {
            if (await parentFolder.TryGetItemAsync(fileName) == null)
                return false;

            return true;
        }
        #endregion
    }
}
