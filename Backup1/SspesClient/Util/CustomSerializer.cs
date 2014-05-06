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
using System.Xml.Serialization;
using System.IO;
using SspesClient.SspesService;

namespace SspesClient.Util
{
    public static class CustomSerializer
    {
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }
        public static Challenge DeserializeChallenge(String xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Challenge));

            StreamReader reader = new StreamReader(xmlString);
            reader.ReadToEnd();
            Challenge battle = (Challenge)serializer.Deserialize(reader);
            reader.Close();

            return battle;
        }

    }
}
