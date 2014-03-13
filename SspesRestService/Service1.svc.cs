using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SspesRestService.util;
using SspesRestService.Classes;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace SspesRestService
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "Service1" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
    public class Service1 : IService1
    {
        private static List<Friend> registeredUsers = new List<Friend>();
        private static PersistanceUnit persisor = new PersistanceUnit();
        private static List<Battle> runningBattles = new List<Battle>();

        public static void AppInitialize()
        {
            // This will get called on startup
            if (registeredUsers.Count < 1)
            {
                registeredUsers = persisor.readUsers();
            }
        } 

        public Friend login(Friend user)
        {
            var regUser = (from u in registeredUsers
                           where u.UserId.Equals(user.UserId) && u.Password.Equals(user.Password)
                           select u).FirstOrDefault();
            if (regUser != null)
            {
                return regUser;
            }
            return null;
        }


        public Friend register(Friend user)
        {
            
            Friend newUser = new Friend() { UserName = user.UserName, Password = user.Password }; // creates GUID
            registeredUsers.Add(newUser);

            return newUser;
        }


        public Friend test(string input)
        {
            Friend testfriend = new Friend() { UserName = input };
            registeredUsers.Add(testfriend);
            persisor.saveUsers(registeredUsers);

            return testfriend;
        }

        public List<Friend> getAllUsers()
        {
            return registeredUsers;
        }

        public bool challenge(Challenge challenge)
        {
            // create new battle on server
            Battle newBattle = new Battle()
            {
                player1 = challenge.ChallengeFrom,
                player2 = challenge.ChallengeTo,
                BattleId = challenge.ChallengeId
            };
            runningBattles.Add(newBattle);

            // notify opponent
            challengePlayerNotification(challenge);

            return true;

        }


        public void challengePlayerNotification(Challenge c)
        {
            try
            {

                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(c.ChallengeTo.PChan);
                sendNotificationRequest.Method = "POST";
                sendNotificationRequest.ContentType = "application/json";
                string rawMessage = JsonConvert.SerializeObject(c);
                byte[] notificationMessage = Encoding.Default.GetBytes(rawMessage);
                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                //sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-NotificationClass", "3");
                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }
                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                // Encode the entire string, and print out the counts and the resulting bytes.
                System.Diagnostics.Debug.WriteLine("Encoding the entire string:");
                //PrintCountsAndBytes(rawMessage, Encoding.UTF7);
                //PrintCountsAndBytes(rawMessage, Encoding.UTF8);
                //PrintCountsAndBytes(rawMessage, Encoding.Unicode);
                //PrintCountsAndBytes(rawMessage, Encoding.BigEndianUnicode);
                //PrintCountsAndBytes(rawMessage, Encoding.UTF32);

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Fail send push");
            }
        }


        public bool updateUser(Friend friend)
        {
            var user = (from u in registeredUsers
                     where u.UserId == friend.UserId
                     select u).FirstOrDefault();
            if (user != null)
            {
                registeredUsers.Remove(user);
                registeredUsers.Add(new Friend() { UserId = friend.UserId, UserName = friend.UserName, PChan = friend.PChan, Password = friend.Password });
            }
            else
            {
                registeredUsers.Add(new Friend() { UserId = friend.UserId, UserName = friend.UserName, PChan = friend.PChan, Password = friend.Password });
            }
            return true;
        }


        public List<Battle> getAllBattlesForUser(Guid userId)
        {
            return null;
        }


        public bool playerMove(string move, Guid playerId, Guid battleId)
        {
            // get battle by id
            var battle = (from b in runningBattles
                          where b.BattleId == battleId
                          select b).FirstOrDefault();
            if (battle != null)
            {
                if (battle.player1.UserId == playerId)
                {
                    battle.player1Move = move;
                }
                else
                {
                    battle.player2Move = move;
                }
            }

            if(!String.IsNullOrEmpty(battle.player1Move) && !String.IsNullOrEmpty(battle.player2Move))
            {
                EndBattleNotify(battle);
            }
            // if both played: notify both

            return true;
        }

        private void EndBattleNotify(Battle battle)
        {
            notifyAfterBattle(battle.player1.PChan, battle);
            notifyAfterBattle(battle.player2.PChan, battle);
        }

        public void notifyAfterBattle(string chan, Battle battle)
        {
            try
            {

                HttpWebRequest sendNotificationRequest = (HttpWebRequest)WebRequest.Create(chan);
                sendNotificationRequest.Method = "POST";
                sendNotificationRequest.ContentType = "application/json";
                string rawMessage = JsonConvert.SerializeObject(battle);
                byte[] notificationMessage = Encoding.Default.GetBytes(rawMessage);
                // Set the web request content length.
                sendNotificationRequest.ContentLength = notificationMessage.Length;
                //sendNotificationRequest.ContentType = "text/xml";
                sendNotificationRequest.Headers.Add("X-NotificationClass", "3");
                using (Stream requestStream = sendNotificationRequest.GetRequestStream())
                {
                    requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                }
                // Send the notification and get the response.
                HttpWebResponse response = (HttpWebResponse)sendNotificationRequest.GetResponse();
                string notificationStatus = response.Headers["X-NotificationStatus"];
                string notificationChannelStatus = response.Headers["X-SubscriptionStatus"];
                string deviceConnectionStatus = response.Headers["X-DeviceConnectionStatus"];

                // Encode the entire string, and print out the counts and the resulting bytes.
                System.Diagnostics.Debug.WriteLine("Encoding the entire string:");
                //PrintCountsAndBytes(rawMessage, Encoding.UTF7);
                //PrintCountsAndBytes(rawMessage, Encoding.UTF8);
                //PrintCountsAndBytes(rawMessage, Encoding.Unicode);
                //PrintCountsAndBytes(rawMessage, Encoding.BigEndianUnicode);
                //PrintCountsAndBytes(rawMessage, Encoding.UTF32);

            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Fail send push");
            }
        }
    }
}
