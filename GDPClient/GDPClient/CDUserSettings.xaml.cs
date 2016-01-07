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
    public sealed partial class CDUserSettings : ContentDialog
    {
        public String Username;
        public String Password;
        public String PasswordConfirm;
        public CDUserSettings()
        {
            this.InitializeComponent();
            usernameBox.Text = AppData.Instance.User.Username;
            if (!String.IsNullOrEmpty(AppData.Instance.User.CertSubject)) {

                addCertGrid.Visibility = Visibility.Collapsed;
                certStack.Visibility = Visibility.Visible;
                currentCertGrid.Visibility = Visibility.Visible;
                issuedToTb.Text = AppData.Instance.User.CertSubject;
                issuedByTb.Text = AppData.Instance.User.CertIssuer;
                dateCertFromTb.Text = AppData.Instance.User.CertValidFrom.ToString();
                dateCertToTb.Text = AppData.Instance.User.CertValidTo.ToString();
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Username = usernameBox.Text;
            Password = passwordBox.Password;
            PasswordConfirm = passwordBoxConfirm.Password;
            
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {

        }

        private void addCertBtn_Click(object sender, RoutedEventArgs e)
        {
            addCertGrid.Visibility = Visibility.Collapsed;
            currentCertGrid.Visibility = Visibility.Collapsed;
            chooseStack.Visibility = Visibility.Visible;
            certStack.Visibility = Visibility.Visible;
        }

        private void removeCertBtn_Click(object sender, RoutedEventArgs e)
        {
            addCertGrid.Visibility = Visibility.Visible;
            certStack.Visibility = Visibility.Collapsed;
        }
    }
}
