using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Category> Categories { get; set; }
        public List<Entry> Entries { get; set; }

        public MainPage()
        {
            Entries = new List<Entry>();
            this.InitializeComponent();
            LoadCategories();
            LoadEntries();
        }

        private async void LoadCategories()
        {
            Categories = new List<Category>();
            //while (AppData.Instance.User == null) ;
            try
            {
                HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                    App.LocalSettings.Values[App.ServiceConn].ToString() + "category",
                    HttpMethod.Get);

                Categories.Add(new Category() { IdCategory = 0, Title = "All" });
                if (mReceived.IsSuccessStatusCode)
                {
                    (JsonConvert.DeserializeObject<List<Category>>(await mReceived.Content.ReadAsStringAsync())).ForEach(p => Categories.Add(p));
                    categoriesList.UpdateLayout();
                }
                else
                {
                    throw new Exception("Unable to load categories");
                }
                mReceived.Dispose();
            }
            catch
            {
                ShowErrorMsg("Unable to load categories.");
            }
        }

        private void LoadEntries()
        {
            Entries = new List<Entry>();
        }

        #region TopBar
        private void SplitViewButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private async void listView1_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            CDEditEntry cdee = new CDEditEntry();
            await cdee.ShowAsync();

            /*if (cdee == CDEditEntry.)
            {

            }*/
            /*if (signInDialog.Result == SignInResult.SignInOK)
            {
                // Sign in was successful.
            }
            else if (signInDialog.Result == SignInResult.SignInFail)
            {
                // Sign in failed.
            }
            else if (signInDialog.Result == SignInResult.SignInCancel)
            {
                // Sign in was cancelled by the user.
            }*/
        }

        private async void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            CDUserSettings cdus = new CDUserSettings();
            await cdus.ShowAsync();
        }


        private async void newEntryAbb_Click(object sender, RoutedEventArgs e)
        {
            CDNewEntry cdne = new CDNewEntry();
            await cdne.ShowAsync();
        }

        private async void editEntryAbb_Click(object sender, RoutedEventArgs e)
        {
            CDEditEntry cdee = new CDEditEntry();
            await cdee.ShowAsync();
        }
        #endregion

        private void OnSelectedItemsChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ShowErrorMsg(String msg)
        {
            var messageDialog = new MessageDialog(msg);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
