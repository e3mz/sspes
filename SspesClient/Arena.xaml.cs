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

namespace SspesClient
{
    public partial class Arena : PhoneApplicationPage
    {
        SspesService.SspesServiceClient mySer = new SspesService.SspesServiceClient();
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
                MessageBox.Show("Unentschieden");
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
            pl1Played = false;
            pl2Played = false;
               
        }

        void announceWinner(User winner)
        {


            if (tbx_playerName.Text.Equals(winner.UserName))
            {
                tbx_playerName.Foreground = new SolidColorBrush(Colors.Green);
                tbx_pl1Score.Text = currentBattle.player1Score.ToString();
            }else
            {
                tbx_opponentName.Foreground = new SolidColorBrush(Colors.Green);
                tbx_pl2Score.Text = currentBattle.player2Score.ToString();
            }
            if (winner.UserId == App.currentUser.UserId)
            {
                MessageBox.Show("Gratulations, You win!");
            }
            else
            {
                MessageBox.Show("You Lose!");
            }

        }


        

    }
}