using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebGymTrivelloniBattaglioli.Models;

namespace WebGymTrivelloniBattaglioli
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "ServiceWCFdb" nel codice, nel file svc e nel file di configurazione contemporaneamente.
    // NOTA: per avviare il client di prova WCF per testare il servizio, selezionare ServiceWCFdb.svc o ServiceWCFdb.svc.cs in Esplora soluzioni e avviare il debug.
    public class ServiceWCFdb : IServiceWCFdb
    { 
        bool IServiceWCFdb.InserisciCliente(ClienteModel cliente)
        {
            string stringConnection = "SERVER=mysql-trivellonibattaglioli.alwaysdata.net;DATABASE=trivellonibattaglioli_webgym;UID=274342_trivebatt; PASSWORD=MauroDiligenti!";
            MySqlConnection connection = new MySqlConnection(stringConnection);
            connection.Open();
            using (MySqlCommand command = connection.CreateCommand())
            {
                string query = "INSERT INTO Persona VALUES(NULL,"+cliente.Nome+ "," + cliente.Cognome + " , " + cliente.Email +
                    ", " + cliente.Data_nascita + ", " + cliente.Telefono + ", " + cliente.Sesso.ToString() + ",cliente , " + cliente.Password + ")";
                MySqlCommand comando = new MySqlCommand(query, connection);
                comando.ExecuteNonQuery();
                ///eseguire operazioni di catalogo, ad esempio per eseguire query sulla struttura di un database o 
                ///per creare oggetti di database, ad esempio tabelle, oppure per modificare i dati in un database 
                ///senza utilizzare un DataSet oggetto eseguendo istruzioni UPDATE, INSERT o DELETE.
                return true;
            }
            return false;
        }
    }
}
