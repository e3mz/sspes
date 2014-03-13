using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServiceHost
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "MyService" sowohl im Code als auch in der Konfigurationsdatei ändern.
    public class MyService : IMyService
    {
        public string DoWork(string value)
        {
            return "Welcome " + value;
        }
    }
}
