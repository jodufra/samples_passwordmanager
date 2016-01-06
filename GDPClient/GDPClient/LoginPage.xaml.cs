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
using System.ComponentModel.DataAnnotations;
using Windows.UI.Popups;
using Windows.Web.Http;
using Newtonsoft.Json;
using Entities;
using Utils;
using Windows.Security.Cryptography.Certificates;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private IReadOnlyList<Certificate> certList;
        private static string NOCERTS = "No certificates found";
        private Certificate selectedCertificate = null;
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void signupBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SignUpPage));
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleLoading(true);

            String error = "";
            if (string.IsNullOrEmpty(usernameBox.Text) || usernameBox.Text.Length < 3)
                error = "Username is required and its length higher or equal to 3." + Environment.NewLine;

            if (string.IsNullOrEmpty(passwordBox.Password))
                error += "Password is required.";

            if (string.IsNullOrEmpty(error))
            {
                try
                {
                    HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                        App.LocalSettings.Values[App.ServiceConn].ToString() + "auth/login",
                        HttpMethod.Post,
                        (new { Username = usernameBox.Text, Password = Security.GetSHA256Hash(passwordBox.Password) }).ToQueryString("login"));

                    if (mReceived.IsSuccessStatusCode)
                    {
                        User user = JsonConvert.DeserializeObject<User>(await mReceived.Content.ReadAsStringAsync());
                        AppData.Instance.User = user;

                        Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        error = JsonConvert.DeserializeObject<String>(await mReceived.Content.ReadAsStringAsync());
                    }
                    mReceived.Dispose();
                }
                catch
                {
                    error = "Unable to request the service.";
                }
            }

            if (!string.IsNullOrEmpty(error))
                ShowErrorMsg(error);

            toggleLoading(false);
        }

        private void toggleLoading(bool isLoading)
        {
            if (isLoading)
            {
                loadingPb.Visibility = Visibility.Visible;
                loginBtn.IsEnabled = false;
                certloginTb.IsEnabled = false;
            }
            else
            {
                loadingPb.Visibility = Visibility.Collapsed;
                loginBtn.IsEnabled = true;
                certloginTb.IsEnabled = true;
            }
        }

        public bool IsValidEmail(string source)
        {
            return new EmailAddressAttribute().IsValid(source);
        }

        private async void ShowErrorMsg(String msg)
        {
            var messageDialog = new MessageDialog(msg);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private async void serviceConnBtn_Click(object sender, RoutedEventArgs e)
        {
            CDServiceConn cdsc = new CDServiceConn();
            await cdsc.ShowAsync(); 
        }

        private void certloginTb_Click(object sender, RoutedEventArgs e)
        {
            if (certloginTb.IsChecked == true)
            {
                loginFields.Visibility = Visibility.Collapsed;
                certFields.Visibility = Visibility.Visible;
                loadCertificates();
            }
            else
            {
                loginFields.Visibility = Visibility.Visible;
                certFields.Visibility = Visibility.Collapsed;
            }
        }

        private void loadCertificates()
        {
            certificatesListCB.Items.Clear();
            var task = CertificateStores.FindAllAsync();
            task.AsTask().Wait();
            var certlist = task.GetResults();

            LoadCertList(certlist);

            if (certificatesListCB.Items.Count == 0)
            {
                certificatesListCB.Items.Add(NOCERTS);
                certificatesListCB.IsEnabled = false;
            }
            else
            {
                certificatesListCB.IsEnabled = true;
            }
            certificatesListCB.SelectedIndex = 0;
        }

        public void LoadCertList(IReadOnlyList<Certificate> certificateList)
        {
            this.certList = certificateList;
            this.certificatesListCB.Items.Clear();

            if (certList.Count > 0)
                certificatesListCB.Items.Add("");
            for (int i = 0; i < certList.Count; i++)
            {
                this.certificatesListCB.Items.Add(certList[i].Subject);
            }
        }

        private void refreshCertsBtn_Click(object sender, RoutedEventArgs e)
        {
            loadCertificates();
        }
    }
}
