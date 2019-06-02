using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Contacts.Services.DialogService
{
    public interface IDialogService
    {
        /// <summary>
        /// Показывает диалоговое окно с информацию о контакте на нем
        /// </summary>
        /// <param name="contactToShow">
        /// Контакт информацию которого нужно показать
        /// </param>
        /// <returns>
        /// Primary - если нажата главная кнопка
        /// Secondary - если нажата второстепенная кнопка
        /// None - если диалоговое окно было закрыто(к примеру клавишой Esc)
        /// </returns>
        Task<ContentDialogResult> ShowDialogAsync(Models.Contacts contactToShow);
    }
}
