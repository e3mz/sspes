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



namespace SspesClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        //// Cached Socket object that will be used by each call for the lifetime of this class
        //Socket _socket = null;

        //// Signaling object used to notify when an asynchronous operation is completed
        //static ManualResetEvent _clientDone = new ManualResetEvent(false);

        //// Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        //// timeout period, the call is aborted.
        //const int TIMEOUT_MILLISECONDS = 5000;

        //// The maximum size of the data buffer to use with the asynchronous socket methods
        //const int MAX_BUFFER_SIZE = 2048;

        SspesService.Service1Client mySer = new SspesService.Service1Client();

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            FunnyNameGenerator gen = new FunnyNameGenerator();
            tbx_userName.Text = gen.getFunnyName();
           mySer.registerCompleted += new EventHandler<SspesService.registerCompletedEventArgs>(mySer_registerCompleted);
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
                SspesService.Friend f = new SspesService.Friend() { UserName = tbx_userName.Text, Password = pw_pw.Password };
                mySer.registerAsync(f);
                
                
            }
            else
            {
                MessageBox.Show("Passwörter müssen übereinstimmen!");
            }
            
            

            //ServiceReference1.SspesWcfServiceClient myService = new ServiceReference1.SspesWcfServiceClient();
            //myService.SayHelloCompleted += new EventHandler<ServiceReference1.SayHelloCompletedEventArgs>(myService_SayHelloCompleted);
            //myService.SayHelloAsync("biber");
            //myService.SayHelloAsync("biber");
            //MessageBox.Show(myService.DoWorkAsync("Hello World!"));
            //myService.Close();


            //Connect("localhost", 8000);


            //if (pw_pw.Password.Equals(pw_pw_wdh.Password))
            //{
            //    App.currentUser = new Friend() { UserName = tbx_userName.Text, Password = pw_pw.Password };
            //    NavigationService.Navigate(new Uri("/Friends.xaml", UriKind.Relative));
            //}
        }

        //public string Connect(string hostName, int portNumber)
        //{
        //    string result = string.Empty;

        //    // Create DnsEndPoint. The hostName and port are passed in to this method.
        //    DnsEndPoint hostEntry = new DnsEndPoint(hostName, portNumber);

        //    // Create a stream-based, TCP socket using the InterNetwork Address Family. 
        //    _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    // Create a SocketAsyncEventArgs object to be used in the connection request
        //    SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
        //    socketEventArg.RemoteEndPoint = hostEntry;

        //    // Inline event handler for the Completed event.
        //    // Note: This event handler was implemented inline in order to make this method self-contained.
        //    socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
        //    {
        //        // Retrieve the result of this request
        //        result = e.SocketError.ToString();

        //        // Signal that the request is complete, unblocking the UI thread
        //        _clientDone.Set();
        //    });

        //    // Sets the state of the event to nonsignaled, causing threads to block
        //    _clientDone.Reset();

        //    // Make an asynchronous Connect request over the socket
        //    _socket.ConnectAsync(socketEventArg);

        //    // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
        //    // If no response comes back within this time then proceed
        //    _clientDone.WaitOne(TIMEOUT_MILLISECONDS);

        //    return result;
        //}

        //void myService_SayHelloCompleted(object sender, ServiceReference1.SayHelloCompletedEventArgs e)
        //{
        //    MessageBox.Show(e.Result);
        //}

       
    }

}