#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.Services.FileStoringService
{
    /// <summary>
    /// Представляет фаловые операций 
    /// </summary>
    public interface IFileStoringService
    {
        /// <summary>
        /// Сохраняет файл в хранилище приложения
        /// </summary>
        /// <param name="parentFolder">
        /// Хранилище в которое нужно сохранить файл 
        /// </param>
        /// <param name="file">
        /// Файл который нужно сохранить
        /// </param>
        /// <param name="fileName">
        /// Имя которое получает файл при сохранение
        /// </param>
        Task SaveToStorage(StorageFolder parentFolder, StorageFile file, string fileName);
    }
}
