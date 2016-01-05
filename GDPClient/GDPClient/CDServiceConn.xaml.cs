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

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    public sealed partial class CDServiceConn : ContentDialog
    {
        public enum Result
        {
            Save,
            Cancel
        }

        public CDServiceConn()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!String.IsNullOrEmpty(serviceTb.Text))
            {
                App.LocalSettings.Values[App.ServiceConn] = serviceTb.Text;
            }

        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            serviceTb.Text = App.LocalSettings.Values[App.ServiceConn].ToString();
        }
    }
}
