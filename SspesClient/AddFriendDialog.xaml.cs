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

namespace SspesClient
{
    public partial class AddFriendDialog : PhoneApplicationPage
    {

        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();

        public AddFriendDialog()
        {
            InitializeComponent();
            mySer.addFriendCompleted += new EventHandler<SspesService.addFriendCompletedEventArgs>(mySer_addFriendCompleted);
            mySer.getFriendsByIdCompleted += new EventHandler<SspesService.getFriendsByIdCompletedEventArgs>(mySer_getFriendsByIdCompleted);
        }

        void mySer_getFriendsByIdCompleted(object sender, SspesService.getFriendsByIdCompletedEventArgs e)
        {
            App.friendsList = e.Result;
            NavigationService.GoBack();
        }

        void mySer_addFriendCompleted(object sender, SspesService.addFriendCompletedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Result.UserName))
            {
                App.currentUser.FriendsList.Add(e.Result.UserId);
                mySer.getFriendsByIdAsync(App.currentUser.UserId.ToString());
            }
            else
            {
                MessageBox.Show("Es konnte kein Spieler mit dem Nutzernamen: " + App.friendToAdd + " gefunden werden! Bitte versuchen sie es erneut.");
            }
        }

        private void btn_addFriend_Click(object sender, RoutedEventArgs e)
        {
            App.friendToAdd = tbx_friendToAdd.Text;
            mySer.addFriendAsync(App.currentUser.UserId.ToString(), App.friendToAdd);
        }
    }
}