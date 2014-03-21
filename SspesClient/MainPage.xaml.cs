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



namespace SspesClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        // HIAI:  http://85.214.221.156:8891/SspesService.svc
        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            FunnyNameGenerator gen = new FunnyNameGenerator();
            tbx_userName.Text = gen.getFunnyName();
            mySer.registerCompleted += new EventHandler<SspesService.registerCompletedEventArgs>(mySer_registerCompleted);

            App.pushChannel = HttpNotificationChannel.Find(App.channelName);
            if (App.pushChannel == null)
            {
                App.pushChannel = new HttpNotificationChannel(App.channelName);
                App.pushChannel.Open();
                App.pushChannel.BindToShellToast();
            }
           
        }

        void mySer_registerCompleted(object sender, SspesService.registerCompletedEventArgs e)
        {
            //MessageBox.Show(e.Result.ToString());

            App.currentUser = e.Result;

            NavigationService.Navigate(new Uri("/Friends.xaml", UriKind.Relative));
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            if (pw_pw.Password.Equals(pw_pw_wdh.Password))
            {
                MessageBox.Show(App.pushChannel.ChannelUri.ToString());
                SspesService.User f = new SspesService.User() { UserName = tbx_userName.Text, Password = pw_pw.Password, PChan = App.pushChannel.ChannelUri.ToString()};
                mySer.registerAsync(f);


            }
            else
            {
                MessageBox.Show("Passwörter müssen übereinstimmen!");
            }



        }



    }

}