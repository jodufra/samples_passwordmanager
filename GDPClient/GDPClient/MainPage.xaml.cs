using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Utils;
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
    public sealed partial class MainPage : Page
    {
        public class MainPageModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public List<Category> Categories { get { return AppData.Instance.Categories; } set { AppData.Instance.Categories = value; NotifyPropertyChanged("Categories"); IdCategoryFilter = 0; } }
            private List<Record> _records { get; set; }
            public List<Record> Records { get { return _idCategoryFilter == 0 ? _records : _records.Where(c => c.IdCategory == _idCategoryFilter).ToList(); } set { _records = value; NotifyPropertyChanged("Records"); } }
            private int _idCategoryFilter;
            public int IdCategoryFilter { get { return _idCategoryFilter; } set { _idCategoryFilter = value; NotifyPropertyChanged("IdCategoryFilter"); NotifyPropertyChanged("Records"); } }

            public MainPageModel()
            {
                _records = new List<Record>();
                _idCategoryFilter = 0;
            }


            private void NotifyPropertyChanged(String propertyName = "")
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (null != handler)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private MainPageModel Model;

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = (Model = new MainPageModel());
            LoadCategories();
            LoadRecords();
        }

        #region Load
        private async void LoadCategories()
        {
            var c = new List<Category>();
            try
            {
                HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                    App.LocalSettings.Values[App.ServiceConn].ToString() + "category",
                    HttpMethod.Get);

                if (mReceived.IsSuccessStatusCode)
                {
                    c.Add(new Category() { IdCategory = 0, Title = "All" });
                    c.AddRange(JsonConvert.DeserializeObject<List<Category>>(await mReceived.Content.ReadAsStringAsync()));
                }
                else
                {
                    throw new Exception("Unable to load categories.");
                }
                mReceived.Dispose();
            }
            catch (Exception e)
            {
                ShowErrorMsg(e.Message);
            }
            Model.Categories = c;
        }

        private async void LoadRecords()
        {
            var c = new List<Record>();
            try
            {
                HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                    App.LocalSettings.Values[App.ServiceConn].ToString() + "record",
                    HttpMethod.Get);

                if (mReceived.IsSuccessStatusCode)
                {
                    c.AddRange(JsonConvert.DeserializeObject<List<Record>>(await mReceived.Content.ReadAsStringAsync()));
                    c.ForEach(r => r.ParseEntry(AppData.Instance.User));
                }
                else
                {
                    throw new Exception();
                }
                mReceived.Dispose();
            }
            catch
            {
                ShowErrorMsg("Unable to load records.");
            }
            Model.Records = c;
        }
        #endregion

        private void categoriesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Model.IdCategoryFilter = Model.Categories[categoriesList.SelectedIndex].IdCategory;
        }

        #region TopBar
        private void SplitViewButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            CallCDUserSettings(new CDUserSettings());
        }
        private async void CallCDUserSettings(CDUserSettings cdus)
        {
            var result = await cdus.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var msgs = new List<String>();
                if (String.IsNullOrEmpty(cdus.Username))
                    msgs.Add("Username is required.");
                if (String.IsNullOrEmpty(cdus.Password))
                    msgs.Add("Password is required.");
                else if (cdus.Password != cdus.PasswordConfirm)
                    msgs.Add("Password and confirmation aren't equal.");

                if (!msgs.Any())
                {
                    try
                    {
                        var update = new
                        {
                            idUser = AppData.Instance.User.IdUser,
                            username = cdus.Username,
                            password = Security.GetSHA256Hash(cdus.Password),
                            certSubject = (string)null,
                            certIssuer = (string)null,
                            certSerialNumber = (string)null,
                            certValidFrom = (string)null,
                            certValidTo = (string)null,
                        };
                        HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                            App.LocalSettings.Values[App.ServiceConn].ToString() + "user/update",
                            HttpMethod.Post, update.ToQueryString("update"));
                        msgs.AddRange(JsonConvert.DeserializeObject<List<String>>(await mReceived.Content.ReadAsStringAsync()));
                        if (mReceived.IsSuccessStatusCode)
                        {
                            ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                            mReceived.Dispose();
                            Logout();
                            return;
                        }
                        mReceived.Dispose();
                    }
                    catch
                    {

                        msgs.Add("Unable to request the service!");
                    }
                }

                if (msgs.Any())
                {
                    ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                    CallCDUserSettings(cdus);
                }
            }

        }

        private void newEntryAbb_Click(object sender, RoutedEventArgs e)
        {
            CallCDNewEntry(new CDNewEntry());
        }
        private async void CallCDNewEntry(CDNewEntry cdne)
        {
            var result = await cdne.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var msgs = new List<String>();
                if (String.IsNullOrEmpty(cdne.EntryTitle))
                    msgs.Add("Title is required.");

                if (String.IsNullOrEmpty(cdne.Username))
                    msgs.Add("Username is required.");

                if (String.IsNullOrEmpty(cdne.Password))
                    msgs.Add("Password is required.");

                if (cdne.SelectedCategory == null)
                    msgs.Add("Category is required.");

                if (!msgs.Any())
                {
                    var record = new Record() { IdUser = AppData.Instance.User.IdUser };
                    record.ParsedEntry = new Entry();
                    record.IdCategory = cdne.SelectedCategory != null ? cdne.SelectedCategory.IdCategory : 0;
                    record.ParsedEntry.Title = cdne.EntryTitle;
                    record.ParsedEntry.Username = cdne.Username;
                    record.ParsedEntry.Password = cdne.Password;
                    record.ParsedEntry.Url = cdne.Url;
                    record.ParsedEntry.Note = cdne.Note;
                    record.CryptEntry(AppData.Instance.User);

                    var model = new
                    {
                        IdCategory = record.IdCategory,
                        IdUser = record.IdUser,
                        Entry = Convert.ToBase64String(record.Entry)
                    };

                    try
                    {
                        HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                            App.LocalSettings.Values[App.ServiceConn].ToString() + "record",
                            HttpMethod.Post, model.ToQueryString("record"));
                        msgs.AddRange(JsonConvert.DeserializeObject<List<String>>(await mReceived.Content.ReadAsStringAsync()));
                        if (mReceived.IsSuccessStatusCode)
                        {
                            ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                            LoadRecords();
                            mReceived.Dispose();
                            return;
                        }
                        mReceived.Dispose();
                    }
                    catch
                    {
                        msgs.Add("Unable to request the service!");
                    }
                }

                if (msgs.Any())
                {
                    ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                    CallCDNewEntry(cdne);
                }
            }
        }

        private void editEntryAbb_Click(object sender, RoutedEventArgs e)
        {
            Record record = (Record)DataGrid.SelectedItem;
            if (record != null)
            {
                CallCDEditEntry(new CDEditEntry(record));
            }
        }
        private async void CallCDEditEntry(CDEditEntry cdee)
        {
            var result = await cdee.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var msgs = new List<String>();
                if (String.IsNullOrEmpty(cdee.EntryTitle))
                    msgs.Add("Title is required.");

                if (String.IsNullOrEmpty(cdee.Username))
                    msgs.Add("Username is required.");

                if (String.IsNullOrEmpty(cdee.Password))
                    msgs.Add("Password is required.");

                if (cdee.SelectedCategory == null)
                    msgs.Add("Category is required.");

                if (!msgs.Any())
                {
                    var record = new Record() { IdUser = AppData.Instance.User.IdUser };
                    record.IdRecord = cdee.IdRecord;
                    record.ParsedEntry = new Entry();
                    record.IdCategory = cdee.SelectedCategory != null ? cdee.SelectedCategory.IdCategory : 0;
                    record.ParsedEntry.Title = cdee.EntryTitle;
                    record.ParsedEntry.Username = cdee.Username;
                    record.ParsedEntry.Password = cdee.Password;
                    record.ParsedEntry.Url = cdee.Url;
                    record.ParsedEntry.Note = cdee.Note;
                    record.CryptEntry(AppData.Instance.User);

                    var model = new
                    {
                        IdRecord = record.IdRecord,
                        IdCategory = record.IdCategory,
                        IdUser = record.IdUser,
                        Entry = Convert.ToBase64String(record.Entry)
                    };

                    try
                    {
                        HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                            App.LocalSettings.Values[App.ServiceConn].ToString() + "record",
                            HttpMethod.Post, model.ToQueryString("record"));
                        msgs.AddRange(JsonConvert.DeserializeObject<List<String>>(await mReceived.Content.ReadAsStringAsync()));
                        if (mReceived.IsSuccessStatusCode)
                        {
                            ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                            LoadRecords();
                            mReceived.Dispose();
                            return;
                        }
                        mReceived.Dispose();
                    }
                    catch
                    {
                        msgs.Add("Unable to request the service!");
                    }
                }

                if (msgs.Any())
                {
                    ShowErrorMsg(String.Join(Environment.NewLine, msgs));
                    CallCDEditEntry(cdee);
                }
            }
        }

        private async void removeEntryAbb_Click(object sender, RoutedEventArgs e)
        {
            Record record = (Record)DataGrid.SelectedItem;
            if (record == null) return;

            var messageDialog = new MessageDialog("Are you sure you want to delete '" + record.ParsedEntry.Title + "'?");
            messageDialog.Commands.Add(new UICommand("No"));
            messageDialog.Commands.Add(new UICommand("Yes"));
            messageDialog.DefaultCommandIndex = 0;
            if ((await messageDialog.ShowAsync()).Label == "No") return;

            try
            {
                HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                    App.LocalSettings.Values[App.ServiceConn].ToString() + "record/" + record.IdRecord,
                    HttpMethod.Delete);

                if (mReceived.IsSuccessStatusCode)
                {
                    ShowErrorMsg("Record removed with success.");
                    LoadRecords();
                }
                else
                {
                    ShowErrorMsg("Error while removing record.");
                }
                mReceived.Dispose();
            }
            catch
            {
                ShowErrorMsg("Unable to remove records.");
            }

        }

        private void copyUserAbb_Click(object sender, RoutedEventArgs e)
        {
            ShowErrorMsg("Not implemented");
        }

        private void copyPasswordAbb_Click(object sender, RoutedEventArgs e)
        {
            ShowErrorMsg("Not implemented");
        }

        private void refreshAbb_Click(object sender, RoutedEventArgs e)
        {
            Model.Records = new List<Record>();
            Model.Categories = new List<Category>();
            LoadCategories();
            LoadRecords();
        }

        #endregion

        private async void ShowErrorMsg(String msg)
        {
            var messageDialog = new MessageDialog(msg);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }

        private async void Logout()
        {
            try
            {
                HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                    App.LocalSettings.Values[App.ServiceConn].ToString() + "auth/logout",
                    HttpMethod.Post);
                mReceived.Dispose();
            }
            catch { }
            AppData.Instance.Categories.Clear();
            AppData.Instance.User = null;
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
