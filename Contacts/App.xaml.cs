using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel;
using Windows.Storage;
using Template10.Common;
using Template10.Controls;
using GalaSoft.MvvmLight.Ioc;
using Contacts.Views;
using Contacts.Services.ContactsRepositoryService;
using Contacts.Services.FileStoringService;
using Contacts.ViewModels;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace Contacts
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : BootStrapper
    {
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

            NavigationServiceFactory(BackButton.Attach, ExistingContent.Include, shell.ContentFrame);

            return new ModalDialog()
            {
                Content = shell
            };
        }

        /// <summary>
        /// Вызывается при запуске приложения
        /// </summary>
        /// <param name="startKind"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            //Code that delete database from local app data
            //StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDB.db");
            //await file.DeleteAsync();
            if (null == await ApplicationData.Current.LocalFolder.TryGetItemAsync("ContactsDB.db"))
            {
                var appDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ContactsDB.db"));
                await appDb.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            SimpleIoc.Default.Register<IFileStoringService, FileStoringService>();
            SimpleIoc.Default.Register<IContactRepositoryService, ContactDBService>();
            SimpleIoc.Default.Register<AddEditPageViewModel>();
            SimpleIoc.Default.Register<ShellViewModel>();
            SimpleIoc.Default.Register<MasterDetailPageViewModel>();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Black;

            NavigationService.Navigate(typeof(MasterDetailPage));

            await Task.CompletedTask;
        }

        public async override Task OnSuspendingAsync(object s, SuspendingEventArgs e, bool prelaunchActivated)
        {
            await ApplicationData.Current.ClearAsync(ApplicationDataLocality.Temporary);
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            if (page is MasterDetailPage)
                return SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();
            else if (page is AddEditPage)
                return SimpleIoc.Default.GetInstance<AddEditPageViewModel>();
            else
                return base.ResolveForPage(page, navigationService);
        }
    }
}
