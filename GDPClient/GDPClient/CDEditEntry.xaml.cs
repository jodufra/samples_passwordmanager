using Entities;
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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    public sealed partial class CDEditEntry : ContentDialog
    {
        public int IdRecord;
        public string EntryTitle;
        public string Username;
        public string Password;
        public string Url;
        public string Note;
        public Category SelectedCategory { get { return (Category)categoryCombo.SelectedItem; } }

        public CDEditEntry(Record record)
        {
            this.InitializeComponent();
            var cats = AppData.Instance.Categories.Where(c => c.IdCategory != 0).ToList();
            int i;
            for (i = 0; i < cats.Count; i++)
            {
                if (cats[i].IdCategory == record.IdCategory) break;
            }
            categoryCombo.ItemsSource = cats;
            categoryCombo.SelectedIndex = i;
            IdRecord = record.IdRecord;
            titleBox.Text = record.ParsedEntry.Title;
            userNameBox.Text = record.ParsedEntry.Username;
            passwordBox.Text = record.ParsedEntry.Password;
            urlBox.Text = record.ParsedEntry.Url;
            noteBox.Text = record.ParsedEntry.Note;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            EntryTitle = titleBox.Text;
            Username = userNameBox.Text;
            Password = passwordBox.Text;
            Url = urlBox.Text;
            Note = noteBox.Text;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
