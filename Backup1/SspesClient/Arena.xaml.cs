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

using User = SspesClient.SspesService.User;
using Battle = SspesClient.SspesService.Battle;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Notification;
using System.IO.IsolatedStorage;
using System.Text;
using SspesClient.SspesService;
using Newtonsoft.Json;

namespace SspesClient
{
    public partial class Arena : PhoneApplicationPage
    {
        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        Battle currentBattle;
        bool pl1Played = false;
        bool pl2Played = false;

        public Arena()
        {
            InitializeComponent();
            tbx_opponentName.Text = App.currentOpponent.UserName;
            tbx_playerName.Text = App.currentUser.UserName;
            currentBattle = new Battle();
            tbx_pl1Score.Text = currentBattle.player1Score.ToString();
            tbx_pl2Score.Text = currentBattle.player2Score.ToString();
            mySer.playerMoveCompleted += new EventHandler<SspesService.playerMoveCompletedEventArgs>(mySer_playerMoveCompleted);
            mySer.loginCompleted += new EventHandler<SspesService.loginCompletedEventArgs>(mySer_loginCompleted);

        }

        void mySer_playerMoveCompleted(object sender, SspesService.playerMoveCompletedEventArgs e)
        {
            tbx_playerName.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void waitForFirstPlayer()
        {
            throw new NotImplementedException();
        }

        private void gameMove_Tap(object sender, GestureEventArgs e)
        {

            String move = ((Ellipse)sender).Tag.ToString();

            App.currentBattle.player1Move = move;
            switch (move)
            {
                case "stone":
                    img_board.Source = new BitmapImage(new Uri(@"/Images/board_stone.png", UriKind.Relative));
                    break;
                case "scissors":
                    img_board.Source = new BitmapImage(new Uri(@"/Images/board_siz.png", UriKind.Relative));
                    break;
                case "paper":
                    img_board.Source = new BitmapImage(new Uri(@"/Images/board_paper.png", UriKind.Relative));
                    break;
                case "lizard":
                    img_board.Source = new BitmapImage(new Uri(@"/Images/board_lizard.png", UriKind.Relative));
                    break;
                case "spock":
                    img_board.Source = new BitmapImage(new Uri(@"/Images/board_spock.png", UriKind.Relative));
                    break;
                default:
                    break;

            }
            //MessageBox.Show("move für battleID: " + App.currentBattle.BattleId);
            mySer.playerMoveAsync(move, App.currentUser.UserId, App.currentBattle.BattleId);


            //if (pl1Played == false)
            //{
            //    currentBattle = new Battle();
            //    currentBattle.player1 = App.currentUser;
            //    currentBattle.player1Move = move;
            //    pl1Played = true;
            //}else
            //{
            //    currentBattle.player2 = App.currentOpponent;
            //    currentBattle.player2Move = move;
            //    pl2Played = true;
            //    showdown();
            //}

        }

        public void showdown()
        {
            if (currentBattle.player1Move == currentBattle.player2Move)
            {
                announceWinner(null);
            }
            else
            {
                switch (currentBattle.player1Move)
                {
                    case "stone":
                        if (App.stoneBeats.Contains(currentBattle.player2Move))
                        {
                            currentBattle.player1Score += 1;
                            announceWinner(currentBattle.player1);
                        }
                        else
                        {
                            currentBattle.player2Score += 1;
                            announceWinner(currentBattle.player2);
                        }
                        break;
                    case "scissors":
                        if (App.scizzorsBeats.Contains(currentBattle.player2Move))
                        {
                            currentBattle.player1Score += 1;
                            announceWinner(currentBattle.player1);
                        }
                        else
                        {
                            currentBattle.player2Score += 1;
                            announceWinner(currentBattle.player2);
                        }
                        break;
                    case "paper":
                        if (App.paperBeats.Contains(currentBattle.player2Move))
                        {
                            currentBattle.player1Score += 1;
                            announceWinner(currentBattle.player1);
                        }
                        else
                        {
                            currentBattle.player2Score += 1;
                            announceWinner(currentBattle.player2);
                        }
                        break;
                    case "lizard":
                        if (App.lizardBeats.Contains(currentBattle.player2Move))
                        {
                            currentBattle.player1Score += 1;
                            announceWinner(currentBattle.player1);
                        }
                        else
                        {
                            currentBattle.player2Score += 1;
                            announceWinner(currentBattle.player2);
                        }
                        break;
                    case "spock":
                        if (App.spockBeats.Contains(currentBattle.player2Move))
                        {
                            currentBattle.player1Score += 1;
                            announceWinner(currentBattle.player1);
                        }
                        else
                        {
                            currentBattle.player2Score += 1;
                            announceWinner(currentBattle.player2);
                        }
                        break;
                    default:
                        break;
                }


            }
           // pl1Played = false;
            //pl2Played = false;

        }

        void announceWinner(User winner)
        {


            //if (tbx_playerName.Text.Equals(winner.UserName))
            //{
            //    tbx_playerName.Foreground = new SolidColorBrush(Colors.Green);
            //    tbx_pl1Score.Text = currentBattle.player1Score.ToString();
            //}
            //else
            //{
            //    tbx_opponentName.Foreground = new SolidColorBrush(Colors.Green);
            //    tbx_pl2Score.Text = currentBattle.player2Score.ToString();

            //}
            if (winner == null)
            {
                MessageBox.Show("Draw! Try again!");
            }
            else if (winner.UserId == App.currentUser.UserId)
            {
                MessageBox.Show("Gratulations, You win!");
            }
            else
            {
                MessageBox.Show("You Lose!");
            }

        }

        void mySer_loginCompleted(object sender, SspesService.loginCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //MessageBox.Show("Login completed!");
                App.currentUser = e.Result;
                tbx_opponentName.Text = App.currentOpponent.UserName;
                tbx_playerName.Text = App.currentUser.UserName;
                ContentPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("User nicht gefunden!");
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //  If we navigated to this page
            // from the MainPage, the DefaultTitle parameter will be "FromMain".  If we navigated here
            // when the secondary Tile was tapped, the parameter will be "FromTile".
            //textBlockFrom.Text = "Navigated here from " + this.NavigationContext.QueryString["NavigatedFrom"];
            String navFrom = "";
            try
            {
                navFrom = this.NavigationContext.QueryString["NavigatedFrom"];
            }
            catch (Exception) { }

            if (!String.IsNullOrEmpty(navFrom) && navFrom.Contains("Toast"))
            {
                //MessageBox.Show("Navigiert über TOAST!");
                ContentPanel.Visibility = Visibility.Collapsed;
                if (App.currentUser != null)
                {
                    //MessageBox.Show("Keine currentUser! Logge neu ein ...");
                    App.pushChannel = HttpNotificationChannel.Find(App.channelName);
                    if (App.pushChannel == null)
                    {
                        App.pushChannel = new HttpNotificationChannel(App.channelName);
                        App.pushChannel.Open();
                        
                        App.pushChannel.BindToShellToast();
                    }
                    App.pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(pushChannel_HttpNotificationReceived);
                    User f = new User()
                    {
                        UserName = settings["UserName"] as string,
                        UserId = new Guid(settings["UserId"] as string),
                        Password = settings["UserPw"] as string,
                        PChan = App.pushChannel.ChannelUri.ToString()
                    };
                    mySer.loginAsync(f);
                }


                User opponent = findUserByName(this.NavigationContext.QueryString["FromName"]);
                if (opponent == null)
                {
                    opponent = new User() { UserId = new Guid(this.NavigationContext.QueryString["FromId"]), UserName = this.NavigationContext.QueryString["FromName"] };
                }

                //MessageBox.Show("Erstelle Gegner: " + opponent.UserName);
                App.currentBattle = new Battle() { BattleId = new Guid(this.NavigationContext.QueryString["ChallengeId"]), player1 = opponent, player2 = App.currentUser };
                App.currentOpponent = opponent;

            }




        }

        private User findUserByName(string username)
        {
            User user = (from u in App.friendsList
                         where u.UserName == username
                         select u).FirstOrDefault();
            if (user == null) return null;
            return user;


        }

        private void btn_info_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("CurrentUser: {0}", App.currentUser.UserName));
            builder.AppendLine(String.Format("CurrentOpponent: {0}", App.currentOpponent.UserName));
            builder.AppendLine(String.Format("CurrentBattle: {0}", App.currentBattle.BattleId));
            MessageBox.Show(builder.ToString());
        }

        void pushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {

            Battle battle = null;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body))
            {
                string inp = reader.ReadToEnd();
                battle = JsonConvert.DeserializeObject<Battle>(inp);
            }

            Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show("Game over: " + battle.BattleId);
                App.currentBattle = battle;
                showdown();
            });

            App.pushChannel.HttpNotificationReceived -= pushChannel_HttpNotificationReceived;
        }
    }
}