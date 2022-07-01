using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebGymTrivelloniBattaglioli.Models;

namespace WebGymTrivelloniBattaglioli
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IServiceWCFdb" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IServiceWCFdb
    {
        [OperationContract]
        bool InserisciCliente(ClienteModel cliente);
    }
}
