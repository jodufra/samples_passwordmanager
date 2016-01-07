using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    public sealed partial class CDNewEntry : ContentDialog
    {
        public string EntryTitle;
        public string Username;
        public string Password;
        public string PasswordConfirm;
        public string Url;
        public string Note;
        public Category SelectedCategory { get { return (Category)categoryCombo.SelectedItem; } }

        public CDNewEntry()
        {
            this.InitializeComponent();
            categoryCombo.ItemsSource = AppData.Instance.Categories.Where(c => c.IdCategory != 0).ToList();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            EntryTitle = titleBox.Text;
            Username = userNameBox.Text;
            Password = passwordBox.Text;
            PasswordConfirm = passwordConfirmBox.Text;
            Url = urlBox.Text;
            Note = noteBox.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        
    }
}
