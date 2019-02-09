using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
using Template10.Controls;

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
            try
            {
                await ApplicationData.Current.LocalFolder.GetFileAsync("ContactsDB.db");
            }
            catch (Exception)
            {
                var appDb = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///ContactsDB.db"));
                await appDb.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            //SimpleIoc.Default.Register<IContactRepositoryService, ContactDBService>();
            //SimpleIoc.Default.Register<ShellViewModel>();
            //SimpleIoc.Default.Register<MasterDetailPageViewModel>();

            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Colors.Black;
            titleBar.ButtonBackgroundColor = Colors.Black;

            NavigationService.Navigate(typeof(MasterDetailPage));

            await Task.CompletedTask;
        }

        //Comment ResolveForPage in cases when you take off responosbility of getting ViewModels 
        //from App.xaml.cs
        //For example if you setting ViewModel in the Views` constructor and using NavigationCacheMode property

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            if(App.Current.ShowShellBackButton == false)
            {
                App.Current.ShowShellBackButton = true;
            }

            return base.ResolveForPage(page, navigationService);
        }
    }
}
