using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.Services.FileStoringService
{
    /// <summary>
    /// Представляет фаловые операций 
    /// </summary>
    public interface IFileStoringService
    {
        Task SaveToLocalStorageAsync(StorageFile file, string fileName);

        Task DeleteFromLocalStorageAsync(StorageFile file);

        Task SaveToTempStorageAsync(StorageFile file, string fileName);

        Task DeleteFromTempStorageAsync(StorageFile file);

        /// <summary>
        /// Возвращает файл из хранилища
        /// </summary>
        /// <param name="folder">
        /// Хранилище из которого нужно вернуть файл
        /// </param>
        /// <param name="fileName">
        /// Имя файла
        /// </param>
        Task<StorageFile> GetFileAsync(StorageFolder parentFolder, string fileName);
    }
}
