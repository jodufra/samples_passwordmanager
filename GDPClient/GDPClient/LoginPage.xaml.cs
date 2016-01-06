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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GDPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private HttpClient httpClient;

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
            //!string.IsNullOrEmpty(emailBox.Text) && IsValidEmail(emailBox.Text)
            if (!string.IsNullOrEmpty(usernameBox.Text) && usernameBox.Text.Length >= 3)
            {
                if (!string.IsNullOrEmpty(passwordBox.Password))
                {
                    try {
                        httpClient = new HttpClient();
                        Uri uri = new Uri(App.LocalSettings.Values[App.ServiceConn].ToString() + "auth/login?login[Username]=" + usernameBox.Text + "&login[Password]=" + passwordBox.Password);
                        HttpRequestMessage mSent = new HttpRequestMessage(HttpMethod.Post, uri);
                        HttpResponseMessage mReceived = await httpClient.SendRequestAsync(mSent, HttpCompletionOption.ResponseContentRead);

                        if (mReceived.IsSuccessStatusCode)
                        {
                            User user = JsonConvert.DeserializeObject<User>(await mReceived.Content.ReadAsStringAsync());
                            AppData.Instance.User = user;

                            Frame.Navigate(typeof(MainPage));
                        }
                        else
                        {
                            String content = JsonConvert.DeserializeObject<String>(await mReceived.Content.ReadAsStringAsync());
                            ShowErrorMsg(content);
                        }

                        mReceived.Dispose();
                        mSent.Dispose();
                        httpClient.Dispose();
                    }
                    catch
                    {
                        ShowErrorMsg("Unable to request the service!");
                    }

                }
                else
                {
                    ShowErrorMsg("Invalid Password!");
                }
            }
            else
            {
                ShowErrorMsg("Invalid Username!");
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
