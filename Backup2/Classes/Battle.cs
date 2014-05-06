using System;
using System.Net;
using System.Runtime.Serialization;


namespace SspesRestService
{
    [DataContract]
    public class Battle
    {
        [DataMember]
        public Guid BattleId { get; set; }
        [DataMember]
        public Friend player1 { get; set; }
        [DataMember]
        public Friend player2 { get; set; }
        [DataMember]
        public String player1Move { get; set; }
        [DataMember]
        public String player2Move { get; set; }
        [DataMember]
        public int player1Score { get; set; }
        [DataMember]
        public int player2Score { get; set; }

        public Battle()
        {
            player1Score = 0;
            player2Score = 0;
        }
    }
}
