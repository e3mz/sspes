using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace SspesRestService.util
{
    public class PersistanceUnit
    {




        public void saveUsers(List<Friend> registeredUsers)
        {

            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(List<Friend>));
            // Erzeugt XML-Datei im angegebenen Pfad
            FileStream str = new FileStream(@"Users.xml", FileMode.OpenOrCreate);
            // Schreibt User-Objelt in XML-Darstellung in XML-Datei
            ser.Serialize(str, registeredUsers);
            // schliesst den stream
            str.Close();
        }

        public List<Friend> readUsers()
        {

            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(List<Friend>));
            // Liest XML-Datei im angegebenen Pfad
            StreamReader sr = new StreamReader(@"Users.xml");
            // erzeugt UserObjekt aus XML
            List<Friend> u = (List<Friend>)ser.Deserialize(sr);
            // schliesst stream
            sr.Close();

            return u;
        }

        // speichert User in XML
        public void saveUser(Friend u)
        {

            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(Friend));
            // Erzeugt XML-Datei im angegebenen Pfad
            FileStream str = new FileStream(@"UserDump.xml", FileMode.OpenOrCreate);
            // Schreibt User-Objelt in XML-Darstellung in XML-Datei
            ser.Serialize(str, u);
            // schliesst den stream
            str.Close();
        }

        public Friend readUser()
        {
            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(Friend));
            // Liest XML-Datei im angegebenen Pfad
            StreamReader sr = new StreamReader(@"UserDump.xml");
            // erzeugt UserObjekt aus XML
            Friend u = (Friend)ser.Deserialize(sr);
            // schliesst stream
            sr.Close();

            return u;
        }

        public void saveBattles(List<Battle> runningBattles)
        {

            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(List<Battle>));
            // Erzeugt XML-Datei im angegebenen Pfad
            FileStream str = new FileStream(@"Battles.xml", FileMode.Create);
            // Schreibt User-Objelt in XML-Darstellung in XML-Datei
            ser.Serialize(str, runningBattles);
            // schliesst den stream
            str.Close();
        }

        public List<Battle> readBattles()
        {

            // Erzeugt XMLSerializer für Datentyp User               
            XmlSerializer ser = new XmlSerializer(typeof(List<Battle>));
            // Liest XML-Datei im angegebenen Pfad
            StreamReader sr = new StreamReader(@"Battles.xml");
            // erzeugt UserObjekt aus XML
            List<Battle> battles = (List<Battle>)ser.Deserialize(sr);
            // schliesst stream
            sr.Close();

            return battles;
        }
    }
}