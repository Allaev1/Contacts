﻿using Contacts.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Contacts.Views.ContentDialogs
{
    public sealed partial class ContactContentDialog : ContentDialog
    {
        public ContactContentDialog()
        {
            this.InitializeComponent();
            ViewModel = SimpleIoc.Default.GetInstance<ContactContentDialogViewModel>();
        }

        public ContactContentDialogViewModel ViewModel { set; get; }
    }
}
