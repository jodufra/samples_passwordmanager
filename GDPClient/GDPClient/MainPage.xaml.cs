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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

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
    }
}
