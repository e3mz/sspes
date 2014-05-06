using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SspesClient.Util;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Phone.Notification;
using System.ServiceModel;
using System.IO.IsolatedStorage;



namespace SspesClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        // HIAI:  http://85.214.221.156:8891/SspesService.svc
        String hiaiAddy = @"http://85.214.221.156:8891/SspesService.svc";
        // local: http://192.168.1.124:8891/SspesService/SspesService.svc
        String localAddy = @"http://192.168.1.124:8891/SspesService/SspesService.svc";

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();
        

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();

            if (settings.Contains("UserId"))
            {
                pw_pw_wdh.Visibility = Visibility.Collapsed;
                tbl_passwordWdh.Visibility = Visibility.Collapsed;

                tbx_userName.Text = (String)settings["UserName"];
                pw_pw.Password = (String)settings["UserPw"];
            }
            else
            {
                FunnyNameGenerator gen = new FunnyNameGenerator();
                tbx_userName.Text = gen.getFunnyName();
            }

            
            mySer.registerCompleted +=new EventHandler<SspesService.registerCompletedEventArgs>(mySer_registerCompleted);
            mySer.loginCompleted += new EventHandler<SspesService.loginCompletedEventArgs>(mySer_loginCompleted);

            App.pushChannel = HttpNotificationChannel.Find(App.channelName);
            if (App.pushChannel == null)
            {
                App.pushChannel = new HttpNotificationChannel(App.channelName);
                App.pushChannel.Open();
                App.pushChannel.BindToShellToast();
            }
           
        }

        void mySer_loginCompleted(object sender, SspesService.loginCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                App.currentUser = e.Result;
                NavigationService.Navigate(new Uri("/Friends.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("User nicht gefunden!");
            }
        }

        void mySer_registerCompleted(object sender, SspesService.registerCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result.ToString());

            App.currentUser = e.Result;

            if (!settings.Contains("UserId"))
            {
                settings.Add("UserId", e.Result.UserId.ToString());
                settings.Add("UserName", tbx_userName.Text);
                settings.Add("UserPw", pw_pw.Password);
            }
            
            settings.Save();
        
            NavigationService.Navigate(new Uri("/Friends.xaml", UriKind.Relative));
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (pw_pw_wdh.Visibility == Visibility.Visible)
            {
                if (pw_pw.Password.Equals(pw_pw_wdh.Password))
                {
                    //MessageBox.Show(App.pushChannel.ChannelUri.ToString());
                    SspesService.User f = new SspesService.User() { UserName = tbx_userName.Text, Password = pw_pw.Password, PChan = App.pushChannel.ChannelUri.ToString() };
                    mySer.registerAsync(f);


                }
                else
                {
                    MessageBox.Show("Passwörter müssen übereinstimmen!");
                }
            }
            else
            {
                SspesService.User f = new SspesService.User() {UserId = new Guid(settings["UserId"].ToString()) ,UserName = tbx_userName.Text, Password = pw_pw.Password, PChan = App.pushChannel.ChannelUri.ToString() };
                mySer.loginAsync(f);
            }



        }

        private void cb_localServer_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();

            //if (cb_localServer.IsChecked == true)
            //{
            //    mySer = new ServiceReference1.SspesServiceClient();
            //}
            //else
            //{
            //    mySer = new SspesService.SspesServiceClient();
            //}
            

            
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            pw_pw_wdh.Visibility = Visibility.Visible;
            tbl_passwordWdh.Visibility = Visibility.Visible;

            settings.Remove("UserName");
            settings.Remove("UserId");
            settings.Remove("UserPw");

        }



    }

}