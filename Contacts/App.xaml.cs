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
using Contacts.Services.ContactsRepositoryService;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using Contacts.ViewModels;
using Template10.Services.NavigationService;
using Contacts.Views;

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

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            SimpleIoc.Default.Register<IContactRepositoryService, ContactRepositoryServiceFake>();
            SimpleIoc.Default.Register<MasterDetailPageViewModel>();

            NavigationService.Navigate(typeof(MasterDetailPage));

            return Task.CompletedTask;
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
