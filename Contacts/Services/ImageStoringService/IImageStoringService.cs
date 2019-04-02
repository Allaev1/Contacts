using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Contacts.Services.ImageStoringService
{
    /// <summary>
    /// 1. Сохраняет в локальное или временное хранилище
    /// 2. Удаляет из локального или временного хранилища
    /// 3. Выбирает фотографию
    /// </summary>
    public interface IImageStoringService
    {
        Task SaveToLocalStorageAsync(StorageFile file, string fileName);

        Task DeleteFromLocalStorageAsync(StorageFile file);

        Task SaveToTempStorageAsync(StorageFile file, string fileName);

        Task DeleteFromTempStorageAsync(StorageFile file);

        /// <summary>
        /// Возвращает фотографию из хранилища
        /// </summary>
        /// <param name="folder">
        /// Хранилище из которого нужно вернуть фотографию
        /// </param>
        /// <param name="fileName">
        /// Имя файла
        /// </param>
        /// <returns>
        /// Файл фотографий
        /// </returns>
        Task<StorageFile> GetImageFileAsync(StorageFolder folder, string fileName);
        //Task GetFileASync(StorageFolder folder, string fileName); 
        //Можно ли убрать из имени метода суффикс Image? Так как из имени интерфейса понятно 
        //что работа будет с файлами фотографий

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<StorageFile> PickAndGetImageFileAsync();
        //Task<StorageFile> GetFileAsync();
    }
}
