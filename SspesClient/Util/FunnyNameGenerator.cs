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

namespace SspesClient.Util
{
    public class FunnyNameGenerator
    {

        Random r = new Random();
        public String[] firstNames = { "Albert", "Erwin", "Galileo", "Isaac", "Nils", "Marie", "Kurt", "Leonardo", "Werner", "Stephen", "Max" };
        public String[] lastNames = { "Einstein", "Schroedinger", "Gelilei", "Newton", "Bohr", "Curie", "Goedel", "DaVinci", "Heisenberg", "Hawking", "Plank" };



        public string getFunnyName()
        {
            return firstNames[r.Next(0, firstNames.Length)] + " " + lastNames[r.Next(0, lastNames.Length)];
        }
    }
}
