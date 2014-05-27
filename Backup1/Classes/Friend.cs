using System;
using System.Net;
using System.Runtime.Serialization;


namespace SspesRestService
{
    [DataContract]
    public class Friend
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public String PChan { get; set; }

        public Friend()
        {
            UserId = Guid.NewGuid();
            PChan = "";
        }

    }
}
