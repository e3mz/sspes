using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Friend = SspesClient.SspesService.Friend;

namespace SspesClient
{
    public class Battle
    {
        public Friend player1 { get; set; }
        public Friend player2 { get; set; }
        public String player1Move { get; set; }
        public String player2Move { get; set; }
        public int player1Score { get; set; }
        public int player2Score { get; set; }

        public Battle()
        {
            player1Score = 0;
            player2Score = 0;
        }
    }
}
