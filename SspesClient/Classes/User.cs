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

namespace SspesClient.Classes
{
    public class User
    {
       
        public Guid UserId { get; set; }
        
        public String UserName { get; set; }
        
        public String Password { get; set; }
        
        public String PChan { get; set; }

        public User()
        {
            UserId = Guid.NewGuid();
            PChan = "";
        }
        

    }
}
