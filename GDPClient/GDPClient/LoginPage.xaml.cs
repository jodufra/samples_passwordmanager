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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

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
            String error = "";
            if (string.IsNullOrEmpty(usernameBox.Text) || usernameBox.Text.Length < 3)
                error = "Invalid Username!" + Environment.NewLine;

            if (string.IsNullOrEmpty(passwordBox.Password))
                error += "Invalid Password!";

            if (string.IsNullOrEmpty(error))
            {
                try
                {
                    HttpResponseMessage mReceived = await Others.ApiRequest.MakeRequest(
                        App.LocalSettings.Values[App.ServiceConn].ToString() + "auth/login",
                        HttpMethod.Post,
                        (new { login = new { Username = usernameBox.Text, Password = passwordBox.Password } }).ToQueryString());

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
                    error = "Unable to request the service!";
                }
            }

            if (!string.IsNullOrEmpty(error))
                ShowErrorMsg(error);
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


        /*String result = RequestApi(@"/api/photos/folder", "GET", myjson);
        photosFiles = JsonConvert.DeserializeObject<List<Category>>(result);

        private static String RequestApi(String url, String method, String json)
        {
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                url = AppSettingsService.WebsiteUrl + url;
                return client.UploadString(url, "POST", json);
            }
        }*/

        private async void serviceConnBtn_Click(object sender, RoutedEventArgs e)
        {
            CDServiceConn cdsc = new CDServiceConn();
            await cdsc.ShowAsync(); //http://localhost:14006/api/

        }

        private void loginBtn_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
