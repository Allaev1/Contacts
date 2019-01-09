using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Template10.Common;
using Template10.Services.NavigationService;
using System.Threading.Tasks;
using Contacts.Views;
using GalaSoft.MvvmLight.Ioc;
using Contacts.ViewModels;
using Contacts.Services.ContactsRepositoryService;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.Storage;

namespace Contacts
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : BootStrapper
    {
        INavigationService navigationService;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Создает корневой Frame
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            Shell shell = new Shell();

            navigationService = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, shell.ContentFrame);

            return shell;
        }

        /// <summary>
        /// Вызывается при запуске приложения
        /// </summary>
        /// <param name="startKind"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (null == await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDB.db"))
            {
                var appDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ContactsDB.db"));
                await appDb.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            SimpleIoc.Default.Register<IContactRepositoryService, ContactDBService>();
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MasterDetailPageViewModel>();

            navigationService.Navigate(typeof(MasterDetailPage));

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Black;

            await Task.CompletedTask;
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            if (page is MasterDetailPage)
                return SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();
            else
                return base.ResolveForPage(page, navigationService);
        }
    }
}
