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
using User = SspesClient.SspesService.User;
using Challenge = SspesClient.SspesService.Challenge;
using Battle = SspesClient.SspesService.Battle;
using Microsoft.Phone.Notification;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Serialization;
using System.IO;



namespace SspesClient
{
    public partial class Friends : PhoneApplicationPage
    {
        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();

        public Friends()
        {
            InitializeComponent();
            mySer.getAllUsersCompleted += new EventHandler<SspesService.getAllUsersCompletedEventArgs>(mySer_getAllUsersCompleted);
            mySer.getAllUsersAsync();
            mySer.challengeCompleted += new EventHandler<SspesService.challengeCompletedEventArgs>(mySer_challengeCompleted);

            tbx_guid.Text = App.currentUser.UserId.ToString();
            tbx_userName.Text = App.currentUser.UserName;

            App.pushChannel = HttpNotificationChannel.Find(App.channelName);
            if (App.pushChannel == null)
            {
                App.pushChannel = new HttpNotificationChannel(App.channelName);
                App.pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(pushChannel_ChannelUriUpdated);
                App.pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(pushChannel_ErrorOccurred);
                App.pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(pushChannel_HttpNotificationReceived);
                App.pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(pushChannel_ShellToastNotificationReceived);
                App.pushChannel.Open();
                // Bind this new channel for toast events.
                App.pushChannel.BindToShellToast();
            }
            else
            {
                App.pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(pushChannel_ChannelUriUpdated);
                App.pushChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(pushChannel_ErrorOccurred);
                App.pushChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(pushChannel_HttpNotificationReceived);
                App.pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(pushChannel_ShellToastNotificationReceived);
                System.Diagnostics.Debug.WriteLine(App.pushChannel.ChannelUri.ToString());
                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    pushChannel.ChannelUri.ToString()));
            }
        }

        void pushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
        {
            StringBuilder message = new StringBuilder();
            string relativeUri = string.Empty;
            String param = e.Collection["wp:Param"];
            String[] singleParams = param.Split('&');
            String oppoIdFrom = singleParams[0].Remove(0, 1);
            String oppoId = oppoIdFrom.Replace("FromId=", "");
            String oppoName = singleParams[1].Replace("FromName=", "");

            Challenge challenge = new Challenge() { ChallengeFrom = findUserByName(oppoName), ChallengeTo = App.currentUser, ChallengeId = new Guid(e.Collection["wp:Text2"]) };

            //Challenge challenge = CustomSerializer.DeserializeChallenge(e.Collection["Battle"]);

            //// Parse out the information that was part of the message.
            //foreach (string key in e.Collection.Keys)
            //{
            //    message.AppendFormat("{0}: {1}\n", key, e.Collection[key]);

            //    if (string.Compare(
            //        key,
            //        "wp:Param",
            //        System.Globalization.CultureInfo.InvariantCulture,
            //        System.Globalization.CompareOptions.IgnoreCase) == 0)
            //    {
            //        relativeUri = e.Collection[key];
            //    }
            //}

            // Display a dialog of all the fields in the toast.
            Dispatcher.BeginInvoke(() =>
            {
                //MessageBox.Show("Channelge from: " + challenge.ChallengeFrom.UserName);
                App.currentBattle = new Battle() { BattleId = challenge.ChallengeId, player1 = challenge.ChallengeFrom, player2 = App.currentUser };
                App.currentOpponent = challenge.ChallengeFrom;
                NavigationService.Navigate(new Uri("/Arena.xaml", UriKind.Relative));





            });
        }

        void pushChannel_HttpNotificationReceived(object sender, HttpNotificationEventArgs e)
        {
            Challenge challenge = null;
            Battle battle = null;

            using (System.IO.StreamReader reader = new System.IO.StreamReader(e.Notification.Body))
            {
                string inp = reader.ReadToEnd();

                if (inp.Contains("Challenge"))
                {
                    challenge = JsonConvert.DeserializeObject<Challenge>(inp);
                }
                else
                {
                    battle = JsonConvert.DeserializeObject<Battle>(inp);
                }
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (challenge != null)
                {
                    MessageBox.Show("Channelge from: " + challenge.ChallengeFrom.UserName);
                    App.currentBattle = new Battle() { BattleId = challenge.ChallengeId, player1 = challenge.ChallengeFrom, player2 = App.currentUser };
                    App.currentOpponent = challenge.ChallengeFrom;
                    NavigationService.Navigate(new Uri("/Arena.xaml", UriKind.Relative));
                }
                else
                {
                    // MessageBox.Show("Game over");
                    App.currentBattle = battle;
                    showdown();

                }

            });


            //Dispatcher.BeginInvoke(() =>
            //    MessageBox.Show(String.Format("Received Notification {0}:\n{1}",
            //        DateTime.Now.ToShortTimeString(), message))
            //        );
        }

        void pushChannel_ErrorOccurred(object sender, NotificationChannelErrorEventArgs e)
        {
            // Error handling logic for your particular application would be here.
            Dispatcher.BeginInvoke(() =>
                MessageBox.Show(String.Format("A push notification {0} error occurred.  {1} ({2}) {3}",
                    e.ErrorType, e.Message, e.ErrorCode, e.ErrorAdditionalData))
                    );
        }

        void pushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {

            Dispatcher.BeginInvoke(() =>
            {
                //btn_sendMsg.Background = new SolidColorBrush(Colors.Green);
                App.currentUser.PChan = e.ChannelUri.ToString();
                //mySer.updateUserAsync(App.currentUser);
                //signIn(this.nick);

                // Display the new URI for testing purposes. Normally, the URI would be passed back to your web service at this point.
                System.Diagnostics.Debug.WriteLine(e.ChannelUri.ToString());

                //MessageBox.Show(String.Format("Channel Uri is {0}",
                //    e.ChannelUri.ToString()));

            });
        }

        void mySer_challengeCompleted(object sender, SspesService.challengeCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                //MessageBox.Show("Waiting for opponent move ...");
                NavigationService.Navigate(new Uri("/Arena.xaml", UriKind.Relative));
            }
        }

        void mySer_getAllUsersCompleted(object sender, SspesService.getAllUsersCompletedEventArgs e)
        {
            App.friendsList = e.Result;
            lb_friends.ItemsSource = App.friendsList;
        }

        private void FillFriends(int count)
        {
            FunnyNameGenerator gen = new FunnyNameGenerator();
            for (int i = 0; i < count; i++)
            {
                User f = new User() { UserName = gen.getFunnyName() };
                App.friendsList.Add(f);
            }
        }

        private void btn_play_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            chellange(btn.Tag.ToString());

        }

        private void chellange(String opponent)
        {
            //MessageBox.Show("Playing against " + opponent);
            App.currentOpponent = findUserByName(opponent);
            App.currentOpponent = (from u in App.friendsList
                                   where u.UserName == opponent
                                   select u).FirstOrDefault();

            Challenge c = new Challenge() { ChallengeFrom = App.currentUser, ChallengeTo = App.currentOpponent, ChallengeId = Guid.NewGuid() };
            App.currentBattle = new Battle() { BattleId = c.ChallengeId, player1 = App.currentUser, player2 = App.currentOpponent };
            mySer.challengeAsync(c);



        }

        private User findUserByName(string username)
        {
            User user = (from u in App.friendsList
                      where u.UserName == username
                      select u).FirstOrDefault();
            if (user == null) return null;
            return user;
            
            
        }

        private void Image_Tap(object sender, GestureEventArgs e)
        {
            Image btn = (Image)sender;
            chellange(btn.Tag.ToString());
        }

        

        public void showdown()
        {
            if (App.currentBattle.player1Move == App.currentBattle.player2Move)
            {
                MessageBox.Show("Unentschieden");
            }
            else
            {
                switch (App.currentBattle.player1Move)
                {
                    case "stone":
                        if (App.stoneBeats.Contains(App.currentBattle.player2Move))
                        {
                            App.currentBattle.player1Score += 1;
                            announceWinner(App.currentBattle.player1);
                        }
                        else
                        {
                            App.currentBattle.player2Score += 1;
                            announceWinner(App.currentBattle.player2);
                        }
                        break;
                    case "scissors":
                        if (App.scizzorsBeats.Contains(App.currentBattle.player2Move))
                        {
                            App.currentBattle.player1Score += 1;
                            announceWinner(App.currentBattle.player1);
                        }
                        else
                        {
                            App.currentBattle.player2Score += 1;
                            announceWinner(App.currentBattle.player2);
                        }
                        break;
                    case "paper":
                        if (App.paperBeats.Contains(App.currentBattle.player2Move))
                        {
                            App.currentBattle.player1Score += 1;
                            announceWinner(App.currentBattle.player1);
                        }
                        else
                        {
                            App.currentBattle.player2Score += 1;
                            announceWinner(App.currentBattle.player2);
                        }
                        break;
                    case "lizard":
                        if (App.lizardBeats.Contains(App.currentBattle.player2Move))
                        {
                            App.currentBattle.player1Score += 1;
                            announceWinner(App.currentBattle.player1);
                        }
                        else
                        {
                            App.currentBattle.player2Score += 1;
                            announceWinner(App.currentBattle.player2);
                        }
                        break;
                    case "spock":
                        if (App.spockBeats.Contains(App.currentBattle.player2Move))
                        {
                            App.currentBattle.player1Score += 1;
                            announceWinner(App.currentBattle.player1);
                        }
                        else
                        {
                            App.currentBattle.player2Score += 1;
                            announceWinner(App.currentBattle.player2);
                        }
                        break;
                    default:
                        break;
                }


            }
            //pl1Played = false;
            //pl2Played = false;

        }

        void announceWinner(User winner)
        {
            if (winner.UserId == App.currentUser.UserId)
            {
                MessageBox.Show("Gratulations, You win!");
            }
            else
            {
                MessageBox.Show("You Lose!");
            }
            App.currentBattle = null;
            App.currentOpponent = null;

            NavigationService.Navigate(new Uri("/Friends.xaml", UriKind.Relative));

        }

        public Battle DeserializeBattle(String xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Battle));

            StreamReader reader = new StreamReader(xmlString);
            reader.ReadToEnd();
            Battle battle = (Battle)serializer.Deserialize(reader);
            reader.Close();

            return battle;
        }
    }
}