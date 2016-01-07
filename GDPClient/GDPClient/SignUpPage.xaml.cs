using Entities;
using GDPClient.Others;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Certificates;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
    {
        private IReadOnlyList<Certificate> certList;
        private static string NOCERTS = "No certificates found";
        private Certificate selectedCertificate = null;

        public SignUpPage()
        {
            this.InitializeComponent();
            loadCertificates();
        }

        private async void okBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleLoading(true);

            List<String> errors = new List<string>();

            if (String.IsNullOrEmpty(usernameBox.Text) || usernameBox.Text.Length < 3)
                errors.Add("Username is required and its length higher or equal to 3.");

            if (String.IsNullOrEmpty(passwordBox.Password))
                errors.Add("Password is required.");
            else if (passwordBox.Password != passwordConfirmBox.Password)
                errors.Add("Passwords are not equal.");

            if (!errors.Any())
            {
                var register = new
                {
                    username = usernameBox.Text,
                    password = Security.GetSHA256Hash(passwordBox.Password),
                    certSubject = selectedCertificate == null ? null : selectedCertificate.Subject,
                    certIssuer = selectedCertificate == null ? null : selectedCertificate.Issuer,
                    certThumbprint = selectedCertificate == null ? null : Security.GetSHA256Hash(CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(selectedCertificate.GetHashValue()))),
                    certSerialNumber = selectedCertificate == null ? null : CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(selectedCertificate.SerialNumber)),
                    certValidFrom = selectedCertificate == null ? null : (DateTime?)selectedCertificate.ValidFrom.DateTime,
                    certValidTo = selectedCertificate == null ? null : (DateTime?)selectedCertificate.ValidTo.DateTime
                };
                try
                {
                    var url = App.LocalSettings.Values[App.ServiceConn].ToString() + "auth/register";
                    HttpResponseMessage mReceived = await ApiRequest.MakeRequest(url, HttpMethod.Post, register.ToQueryString("register"));
                    errors.AddRange(JsonConvert.DeserializeObject<List<String>>(await mReceived.Content.ReadAsStringAsync()));
                    if (mReceived.StatusCode == HttpStatusCode.Ok)
                    {
                        ShowErrorMsg(String.Join(Environment.NewLine, errors));
                        Frame.Navigate(typeof(LoginPage));
                    }
                    mReceived.Dispose();
                }
                catch
                {
                    errors.Add("Unable to request the service!");
                }
            }

            if (errors.Any())
                ShowErrorMsg(String.Join(Environment.NewLine, errors));

            toggleLoading(false);
        }

        private void toggleLoading(bool isLoading)
        {
            if (isLoading)
            {
                loadingPb.Visibility = Visibility.Visible;
                okBtn.IsEnabled = false;
                cancelBtn.IsEnabled = false;
            }
            else
            {
                loadingPb.Visibility = Visibility.Collapsed;
                okBtn.IsEnabled = true;
                cancelBtn.IsEnabled = true;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //Go back to login page
            Frame.Navigate(typeof(LoginPage));
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

        private void certificatesListCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (certificatesListCB.SelectedIndex == 0)
            {
                certDataGrid.Visibility = Visibility.Collapsed;
                selectedCertificate = null;
            }
            else if (certificatesListCB.SelectedIndex > 0)
            {
                //get cert info
                selectedCertificate = certList[certificatesListCB.SelectedIndex - 1];

                issuedToTb.Text = " " + selectedCertificate.Subject;
                issuedByTb.Text = " " + selectedCertificate.Issuer;
                //CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(selectedcertificate.GetHashValue()));
                //CryptographicBuffer.EncodeToHexString(CryptographicBuffer.CreateFromByteArray(selectedcertificate.SerialNumber));
                dateCertFromTb.Text = " " + selectedCertificate.ValidFrom.ToString("dd/MM/yy H:mm:ss");
                dateCertToTb.Text = " " + selectedCertificate.ValidTo.ToString("dd/MM/yy H:mm:ss");
                certDataGrid.Visibility = Visibility.Visible;
            }
        }

        private async void ShowErrorMsg(String msg)
        {
            var messageDialog = new MessageDialog(msg);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            await messageDialog.ShowAsync();
        }

        private void usernameBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            executeSigUp(sender, e);
        }

        private void passwordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            executeSigUp(sender, e);
        }

        private void passwordConfirmBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            executeSigUp(sender, e);
        }

        private void executeSigUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                okBtn_Click(sender, e);
        }
    }
}
