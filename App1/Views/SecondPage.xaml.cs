using System;

using App1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App1.Views
{
    public sealed partial class SecondPage : Page
    {
        public SecondViewModel ViewModel { get; } = new SecondViewModel();

        public SecondPage()
        {
            InitializeComponent();
        }
    }
}
