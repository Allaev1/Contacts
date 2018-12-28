using System;

using App1.ViewModels;

using Windows.UI.Xaml.Controls;

namespace App1.Views
{
    public sealed partial class FirstPage : Page
    {
        public FirstViewModel ViewModel { get; } = new FirstViewModel();

        public FirstPage()
        {
            InitializeComponent();
        }
    }
}
