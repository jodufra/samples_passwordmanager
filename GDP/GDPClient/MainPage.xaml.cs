using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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
            
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void userEntrieslv_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //Disable stuff
            disableEnableControls(false);
            modalDialogNE.Visibility = Windows.UI.Xaml.Visibility.Visible;
            /*var dialog = new MessageDialog("Are you sure?");
            dialog.Title = "Really?";
            dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
            var res = await dialog.ShowAsync();

            if ((int)res.Id == 0)
            {  
            
            }*/
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            modalDialogNE.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            disableEnableControls(true);
        }

        private void disableEnableControls(bool action)
        {
            newAbb.IsEnabled = action;
            editAbb.IsEnabled = action;
            cpyUserAbb.IsEnabled = action;
            cpyPassAbb.IsEnabled = action;
            userEntrieslv.IsEnabled = action;
            SlidePane.LeftPane.IsEnabled = action;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            SlidePane.IsLeftPaneOpen = false;
            modalDialogAS.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void cancelBtnAS_Click(object sender, RoutedEventArgs e)
        {
            modalDialogAS.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

    }
}
