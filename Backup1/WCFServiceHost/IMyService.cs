using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFServiceHost
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IMyService" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string DoWork(string value);
    }
}
