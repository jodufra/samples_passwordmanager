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
using Windows.Security.Cryptography.Certificates;
using GDPClient.GDPService;
using Windows.UI.Popups;

using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Diagnostics;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page
    {

        IReadOnlyList<Certificate> certList;

        public SignUpPage()
        {
            this.InitializeComponent();

            //--
            //certificatesListCB.Items.Clear();
            var task = CertificateStores.FindAllAsync();
            task.AsTask().Wait();
            var certlist = task.GetResults();
            LoadCertList(certlist);
            
            if (certificatesListCB.Items.Count == 0)
            {
                certificatesListCB.IsEnabled = false;
                certificatesListCB.Items.Add("No certificates found");
            }
            else
            {
                certificatesListCB.IsEnabled = true;
            }
            certificatesListCB.SelectedIndex = 0;
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void okBtn_Click(object sender, RoutedEventArgs e)
        {
            //service teste
            GDPService.GDPServiceClient service = new GDPService.GDPServiceClient();
            var a = service.LoginAsync("asd", "asd");
            var dialog = new MessageDialog(a.ToString());
            var d = await dialog.ShowAsync();
        }

        public async void LoadCertList(IReadOnlyList<Certificate> certificateList)
        {
            this.certList = certificateList;
            this.certificatesListCB.Items.Clear();

            for (int i = 0; i < certList.Count; i++)
            {
                this.certificatesListCB.Items.Add(certList[i].Subject);
            }
        }

       
    }
}
