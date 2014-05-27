using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SspesRestService.Classes
{
    [DataContract]
    public class Challenge
    {
        [DataMember]
        public Guid ChallengeId { get; set; }
        [DataMember]
        public Friend ChallengeFrom { get; set; }
        [DataMember]
        public Friend ChallengeTo { get; set; }
    }
}