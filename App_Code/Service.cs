using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service" nel codice, nel file svc e nel file di configurazione contemporaneamente.
public class Service : IService
{
	const string stringConnection = "SERVER=mysql-trivellonibattaglioli.alwaysdata.net;DATABASE=trivellonibattaglioli_webgym;UID=274342_trivebatt; PASSWORD=MauroDiligenti!";
	public bool InserisciCliente(string codice_fiscale,string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso)
    {
        try
        {
			MySqlConnection connection = new MySqlConnection(stringConnection);
			connection.Open();
			string query = "INSERT INTO Persona VALUES('" + codice_fiscale + "','" + nome + "','" + cognome + "' , '" + mail +
			"', '" + data_nascita + "', '" + telefono + "', '" + sesso + "','cliente' , '" + password + "')";
			MySqlCommand comando = new MySqlCommand(query, connection);
			comando.ExecuteNonQuery();
			connection.Close();
			///eseguire operazioni di catalogo, ad esempio per eseguire query sulla struttura di un database o 
			///per creare oggetti di database, ad esempio tabelle, oppure per modificare i dati in un database 
			///senza utilizzare un DataSet oggetto eseguendo istruzioni UPDATE, INSERT o DELETE.
			return true;
		}
		catch (Exception ex)
        {
			return false;
        }
	}

	public List<ContractDTO> GetAvailableContracts()
    {
		List<ContractDTO> contracts = new List<ContractDTO>();
		MySqlConnection connection = new MySqlConnection(stringConnection);
        try
        {
			connection.Open();
			string query = "SELECT * FROM Contratto";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					int id = (int)reader["id"];
					string descrizione = reader.GetString("descrizione");
					double prezzo = reader.GetDouble("prezzo");
					int durata = (int)reader["durata"];
					ContractDTO app = new ContractDTO(id, descrizione, prezzo, durata); ///SFRUTTO UNA CLASSE 
																						///D'APPOGGIO CON STESSE CARATTERISTICHE DELLA CLASSE CONTRATTOMODEL, 
																						///IN MODO DA POTER INSERIRE NEL GENERICO OBJ I DATI NECESSARI
					contracts.Add(app);
				}
			}
			connection.Close();
			return contracts;
		}
		catch(Exception ex)
        {
			return null;
        }
	}

	public bool AddContractToClient(int id, string codice_fiscale,string startDate)
    {
		MySqlConnection connection = new MySqlConnection(stringConnection);
        try
        {
			connection.Open();
			string query = "INSERT INTO Iscrizione VALUES(NULL,'" + startDate + "','" + codice_fiscale + "','" + id + "')";
			MySqlCommand comando = new MySqlCommand(query, connection);
			comando.ExecuteNonQuery();
			connection.Close();
			return true;
		}
		catch (Exception ex)
        {
			return false;
        }
    }

	/// DANI STASERA QUANDO FAI LA SELECT COUNT USA QUESTO METODO COME ESEMPIO!!!!
	public bool AlreadyRegistered(string codice_fiscale, string mail)
    {
		bool returned = true;
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
        {
			connection.Open();
			string query = "SELECT COUNT(*) FROM Persona WHERE codice_fiscale = '" + codice_fiscale + "'";
			MySqlCommand comando = new MySqlCommand(query, connection);
			int result1 = Convert.ToInt32(comando.ExecuteScalar());
			query = "SELECT COUNT(*) FROM Persona WHERE email = '" + mail + "'";
			comando = new MySqlCommand(query, connection);
			int result2 = Convert.ToInt32(comando.ExecuteScalar());
			connection.Close();
			if (result1 == 0 && result2 == 0)
				returned = false;
			return returned;
		}
		catch(Exception ex)
        {
			return true;
        }
    }

	public bool ConvalidLogIn(string mail, string password)
    {
		bool exist = false;
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
        {
			connection.Open();
			string query = "SELECT COUNT(*) FROM Persona WHERE email = '" + mail + "' AND password = '" + password + "'";
			MySqlCommand comando = new MySqlCommand(query, connection);
			int result = Convert.ToInt32(comando.ExecuteScalar());
			connection.Close();
			if (result == 1)
				exist = true;
			return exist;
		}
		catch (Exception ex)
        {
			return false;
        }
		
    }

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}
}
