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
	public bool InserisciCliente(string codice_fiscale, string nome, string cognome, string mail, string data_nascita, string telefono, string password, string sesso)
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
		catch (Exception ex)
		{
			return null;
		}
	}


	public bool AddContractToClient(int id, string codice_fiscale, string startDate)
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
		catch (Exception ex)
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
			string query = "SELECT COUNT(*) FROM Persona WHERE email = '" + mail + "' AND password = '" + password + "' AND ruolo = 'cliente'";
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

	public bool CercaPersonalTrainerNelDB(string email, string password)
	{
		bool exist = false;
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT COUNT(*) FROM Persona WHERE email='" + email + "' AND password='" + password + "' AND ruolo = 'trainer'";
			MySqlCommand command = new MySqlCommand(query, connection);
			int result = Convert.ToInt32(command.ExecuteScalar());
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

	public UserDTO GetUserByEmail(string mail)
	{
		UserDTO user = new UserDTO();
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT * FROM Persona WHERE email = '" + mail + "'";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string codice_fiscale = reader.GetString("codice_fiscale");
					string nome = reader.GetString("nome");
					string cognome = reader.GetString("cognome");
					string email = reader.GetString("email");
					DateTime data_nascita = reader.GetDateTime("data_nascita");
					string telefono = reader.GetString("telefono");
					string sesso = reader.GetString("sesso");
					string ruolo = reader.GetString("ruolo");
					string password = reader.GetString("password");
					user = new UserDTO(codice_fiscale, nome, cognome, email, data_nascita, telefono, sesso, ruolo, password);
				}
			}
			connection.Close();
			return user;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public ContractDTO GetUserActiveContract(string codice_fiscale)
	{
		List<ContractDTO> contracts = new List<ContractDTO>();
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT Contratto.id,Contratto.descrizione,Contratto.prezzo,Contratto.durata,Iscrizione.data_inizio FROM Contratto JOIN Iscrizione ON (Contratto.id = Iscrizione.idContratto) JOIN Persona ON (Persona.codice_fiscale = Iscrizione.codiceFiscale) WHERE Persona.codice_fiscale = '" + codice_fiscale + "'";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					int id = reader.GetInt32("id");
					string descrizione = reader.GetString("descrizione");
					double prezzo = reader.GetDouble("prezzo");
					int durata = reader.GetInt32("durata");
					DateTime data_inizio = reader.GetDateTime("data_inizio");
					ContractDTO contract = new ContractDTO(id, descrizione, prezzo, durata, data_inizio);
					contracts.Add(contract);
				}
			}
			connection.Close();
			int index = -1;
			int i = 0;
			foreach (ContractDTO contract in contracts)
			{
				if (contract.Data_inizio.AddMonths(contract.Durata).CompareTo(DateTime.Now) >= 0)
				{
					index = i;
					break;
				}
				i++;
			}
			if (index != -1)
				return contracts[index];
			return null;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public List<SchedaDTO> GetSchedeUtente(string cod_fiscale)
	{
		List<SchedaDTO> schede = new List<SchedaDTO>();
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT Assegnazione.data_assegnazione,Assegnazione.inUso,Associazione.commento,Associazione.numeroRipetizioni,Associazione.recupero, Scheda.durata,Scheda.titolo,Scheda.id,cliente.email AS mail_cliente,trainer.email AS mail_trainer,Esercizio.descrizione,Esercizio.immagine FROM Assegnazione JOIN Persona AS cliente ON cliente.codice_fiscale = Assegnazione.cod_cliente JOIN Persona AS trainer ON trainer.codice_fiscale = Assegnazione.cod_trainer JOIN Scheda ON Scheda.id = Assegnazione.id_scheda JOIN Associazione ON Associazione.idScheda = Assegnazione.id_scheda JOIN Esercizio ON Esercizio.id = Associazione.idEsercizio WHERE cliente.codice_fiscale = '" + cod_fiscale + "' ORDER BY Scheda.id";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				///controllo se al cliente non è mai stata associata una scheda
				if (!reader.HasRows) //verifica che la query non abbia righe di ritorno
					return null;
				int segnaposto = -1;
				SchedaDTO scheda = new SchedaDTO();
				for (int i = 1; reader.Read(); i++)
				{
					int id_scheda = reader.GetInt32("id");
					if (i == 1) ///primo giro
					{
						segnaposto = id_scheda;
						scheda = new SchedaDTO(id_scheda, reader.GetString("titolo"), reader.GetInt32("durata"), reader.GetDateTime("data_assegnazione"), reader.GetString("mail_cliente"), reader.GetString("mail_trainer"), reader.GetBoolean("inUso"), new List<EsercizioDTO>(), new List<CaratteristicaEsercizioDTO>());
					}
					EsercizioDTO esercizio = new EsercizioDTO(reader.GetString("descrizione"), reader.GetString("immagine"));
					CaratteristicaEsercizioDTO caratteristiche = new CaratteristicaEsercizioDTO(reader.GetInt32("numeroRipetizioni"), reader.GetTimeSpan("recupero"), reader.GetString("commento"));
					if (segnaposto == id_scheda)
					{
						scheda.Esercizi.Add(esercizio);
						scheda.Caratteristica_esercizi.Add(caratteristiche);
					}
					else
					{
						segnaposto = id_scheda;
						schede.Add(scheda);
						scheda = new SchedaDTO(id_scheda, reader.GetString("titolo"), reader.GetInt32("durata"), reader.GetDateTime("data_assegnazione"), reader.GetString("mail_cliente"), reader.GetString("mail_trainer"), reader.GetBoolean("inUso"), new List<EsercizioDTO>(), new List<CaratteristicaEsercizioDTO>());
						scheda.Esercizi.Add(esercizio);
						scheda.Caratteristica_esercizi.Add(caratteristiche);
					}
				}
				schede.Add(scheda); ///per prendere anche l'ultima scheda
			}
			connection.Close();
			return schede;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	/// Il metodo restituisce una lista di UserDTO contenente tutti i clienti
	/// ai quali il trainer ha assegnato, almeno una volta, la scheda.
	public List<UserDTO> CercaClientiAssociatiAlTrainer(string cod_fiscale_trainer)
	{
		List<UserDTO> Clienti = new List<UserDTO>();
		MySqlConnection connection = new MySqlConnection(stringConnection);

		try
		{
			connection.Open();
			string query = "SELECT * FROM Persona JOIN Assegnazione ON(Persona.codice_fiscale = Assegnazione.cod_cliente) WHERE Assegnazione.cod_trainer ='" + cod_fiscale_trainer + "'";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string codice_fiscale = reader.GetString("codice_fiscale");
					string nome = reader.GetString("nome");
					string cognome = reader.GetString("cognome");
					string email = reader.GetString("email");
					DateTime data_nascita = reader.GetDateTime("data_nascita");
					string telefono = reader.GetString("telefono");
					string sesso = reader.GetString("sesso");
					string ruolo = reader.GetString("ruolo");
					string password = reader.GetString("password");
					UserDTO c = new UserDTO(codice_fiscale, nome, cognome, email, data_nascita, telefono, sesso, ruolo, password);
					Clienti.Add(c);
				}
			}
			connection.Close();
			return Clienti;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public bool InserisciSchedaNelDB(string titolo, int durata)
	{
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string durataString = durata.ToString();
			string query = "INSERT INTO Scheda VALUES(NULL,'" + titolo + "'," + durataString + ")";

			MySqlCommand comando = new MySqlCommand(query, connection);
			comando.ExecuteNonQuery();
			connection.Close();
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
		return false;
	}

	public bool AggiornaUtilizzoSchede(string cod_fiscale_cliente)
	{
		//settare inUso=0 nella tabella Assegnazione per tutte le schede assegnate dal trainer al cliente
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "UPDATE Assegnazione SET inUso = 0 WHERE cod_cliente ='" + cod_fiscale_cliente + "'";
			MySqlCommand comando = new MySqlCommand(query, connection);
			comando.ExecuteNonQuery();
			connection.Close();
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
		return false;
	}

	public int OttieniIdUtimaSchedaInserita()
	{
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			int v = -1;
			string query = "SELECT MAX(Scheda.id) FROM Scheda";
			MySqlCommand command = new MySqlCommand(query, connection);
			v = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();
			return v;
		}
		catch (Exception ex)
		{
			return -1;
		}
	}

	public bool AggiungiNuovaAssegnazione(string cod_fiscale_trainer, string cod_fiscale_cliente, int idScheda, string data)
	{
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string idSchedaStringa = idScheda.ToString();
			string query = "INSERT INTO Assegnazione VALUES(NULL, '" + data + "',1, '" + cod_fiscale_cliente + "','" + cod_fiscale_trainer + "'," + idSchedaStringa + ")";
			MySqlCommand command = new MySqlCommand(query, connection);
			command.ExecuteNonQuery();
			connection.Close();
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	private int OttieniIdUtimoEsercizioInserito()
	{
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			int v = -1;
			string query = "SELECT MAX(Esercizio.id) FROM Esercizio";
			MySqlCommand command = new MySqlCommand(query, connection);
			v = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();
			return v;
		}
		catch (Exception ex)
		{
			return -1;
		}
	}

	public bool AddNewExerciseToCardGym(int id_scheda, string descrizione, int num_ripetizioni, TimeSpan tempo_recupero, string commento, string immagine)
	{
		string tempo = tempo_recupero.ToString(@"hh\:mm\:ss");
		int last_id = OttieniIdUtimoEsercizioInserito();
		if (last_id != -1)
		{
			int new_id = last_id + 1;
			using (MySqlConnection connection = new MySqlConnection(stringConnection))
			{
				connection.Open();
				MySqlCommand command = connection.CreateCommand();
				MySqlTransaction transaction;
				// Start a local transaction.
				transaction = connection.BeginTransaction();
				command.Connection = connection;
				command.Transaction = transaction;
				try
				{
					///per avere la certezza che il nuovo elemento sia inserito con id = new_id
					command.CommandText = "ALTER TABLE Esercizio AUTO_INCREMENT = " + new_id.ToString() + ";";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Esercizio VALUES (NULL,'" + immagine + "','" + descrizione + "');";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Associazione VALUES (" + id_scheda.ToString() + "," + new_id.ToString() + "," + num_ripetizioni.ToString() + ",'" + tempo + "','" + commento + "');";
					command.ExecuteNonQuery();

					// Attempt to commit the transaction.
					transaction.Commit();
					connection.Close();
					return true;
				}
				catch (Exception ex)
				{
					// Attempt to roll back the transaction.
					try
					{
						transaction.Rollback();
						return false;
					}
					catch (Exception ex2)
					{
						return false;
					}
				}
			}
		}
		else
			return false;
	}

	private int getExerciseId(string descrizione)
	{
		int id;
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT Esercizio.id FROM Esercizio WHERE Esercizio.descrizione = '" + descrizione + "'";
			MySqlCommand command = new MySqlCommand(query, connection);
			id = Convert.ToInt32(command.ExecuteScalar());
			connection.Close();
			return id;
		}
		catch (Exception ex)
		{
			return -1;
		}
	}

	public bool OttieniEserciziAssociatiAllaScheda(int id_scheda, ref List<EsercizioDTO> lista_esercizi, ref List<CaratteristicaEsercizioDTO> lista_caratteristiche)
	{
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT Esercizio.immagine, Esercizio.descrizione, Associazione.numeroRipetizioni, Associazione.recupero, Associazione.commento FROM Esercizio, Associazione,Scheda WHERE Associazione.idEsercizio = Esercizio.id AND Associazione.idScheda = Scheda.id AND Scheda.id = " + id_scheda.ToString() + " ORDER BY Esercizio.id";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string descrizione = reader.GetString("descrizione");
					string immagine = reader.GetString("immagine");
					int ripetizioni = reader.GetInt32("numeroRipetizioni");
					TimeSpan recupero = reader.GetTimeSpan("recupero");
					string commento = reader.GetString("commento");
					EsercizioDTO esercizio = new EsercizioDTO(descrizione, immagine);
					CaratteristicaEsercizioDTO caratteristicha = new CaratteristicaEsercizioDTO(ripetizioni, recupero, commento);
					lista_esercizi.Add(esercizio);
					lista_caratteristiche.Add(caratteristicha);
				}
			}
			connection.Close();
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	public List<EsercizioDTO> GetAllExercise()
	{
		List<EsercizioDTO> listaEsercizi = new List<EsercizioDTO>();
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT Esercizio.id, Esercizio.descrizione,Esercizio.immagine FROM Esercizio";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string descrizione = reader.GetString("descrizione");
					string immagine = reader.GetString("immagine");
					int id = reader.GetInt32("id");
					EsercizioDTO esercizio = new EsercizioDTO(descrizione, immagine);
					listaEsercizi.Add(esercizio);
				}
			}
			connection.Close();
			return listaEsercizi;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public bool AddExerciseToCardGym(string descrizione_esercizio, int id_scheda, int num_ripetizioni, TimeSpan tempo_recupero, string commento)
	{
		string tempo = tempo_recupero.ToString(@"hh\:mm\:ss");
		try
		{
			MySqlConnection connection = new MySqlConnection(stringConnection);
			connection.Open();
			EsercizioDTO selectedExercise = getExerciseByDescrizione(descrizione_esercizio);
			if (selectedExercise != null)
			{
				int exercise_id = getExerciseId(descrizione_esercizio);
				string query = "INSERT INTO Associazione VALUES(" + id_scheda.ToString() + ", " + exercise_id.ToString() + "," + num_ripetizioni.ToString() + ", '" + tempo + "','" + commento + "')";
				MySqlCommand command = new MySqlCommand(query, connection);
				command.ExecuteNonQuery();
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	private EsercizioDTO getExerciseByDescrizione(string descrizione)
	{
		EsercizioDTO result = null;
		MySqlConnection connection = new MySqlConnection(stringConnection);
		try
		{
			connection.Open();
			string query = "SELECT * FROM Esercizio WHERE Esercizio.descrizione = '" + descrizione + "'";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string descrizionee = reader.GetString("descrizione");
					string immagine = reader.GetString("immagine");
					int id = reader.GetInt32("id");
					result = new EsercizioDTO(descrizione, immagine);
				}
			}
			connection.Close();
			return result;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public List<UserDTO> OttieniListaTrainer()
	{
		try
		{
			MySqlConnection connection = new MySqlConnection(stringConnection);
			connection.Open();
			List<UserDTO> lista_trainer = new List<UserDTO>();
			string query = "SELECT Persona.codice_fiscale, Persona.Nome, Persona.Cognome FROM Persona WHERE ruolo='trainer'";
			MySqlCommand command = new MySqlCommand(query, connection);
			using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
			{
				while (reader.Read())
				{
					string codFiscale = reader.GetString("codice_fiscale");
					string nome = reader.GetString("nome");
					string cognome = reader.GetString("cognome");
					UserDTO tmpUser = new UserDTO(codFiscale, nome, cognome);
					lista_trainer.Add(tmpUser);
				}
			}
			connection.Close();
			return lista_trainer;
		}
		catch (Exception ex)
		{
			return null;
		}
	}

	public bool AssegnaPrimaSchedaAlCliente(string cod_fiscale_cliente, string cod_fiscale_trainer, string nomeCognomeCliente)
	{
		using (MySqlConnection connection = new MySqlConnection(stringConnection))
		{
			connection.Open();
			try
			{
				string query = "SELECT MAX(Scheda.id) FROM Scheda;";
				MySqlCommand command = new MySqlCommand(query, connection);
				int idScheda = Convert.ToInt32(command.ExecuteScalar()); //ottengo l'id della scheda
				idScheda++;
				MySqlCommand command2 = connection.CreateCommand();
				MySqlTransaction transaction;
				// Start a local transaction.
				transaction = connection.BeginTransaction();
				command2.Connection = connection;
				command2.Transaction = transaction;
				///per avere la certezza che il nuovo elemento sia inserito con id = new_id
				try
				{

					command2.CommandText = "ALTER TABLE Scheda AUTO_INCREMENT = " + idScheda.ToString() + ";";
					command2.ExecuteNonQuery();
					command2.CommandText = "INSERT INTO Scheda VALUES(NULL, 'Nuova scheda per " + nomeCognomeCliente + "', 1);";
					command2.ExecuteNonQuery();
					/*command.CommandText = "SELECT MAX(Esercizio.id) FROM Esercizio;";
					int idScheda = Convert.ToInt32(command.ExecuteScalar());*/
					command2.CommandText = "INSERT INTO Associazione VALUES(" + idScheda.ToString() + ",1,10,'00:01:30','Movimento controllato x3 serie');";
					command2.ExecuteNonQuery();
					command2.CommandText = "INSERT INTO Associazione VALUES(" + idScheda.ToString() + ",2,8,'00:01:30','x4 serie');";
					command2.ExecuteNonQuery();
					command2.CommandText = "INSERT INTO Associazione VALUES(" + idScheda.ToString() + ",3,12,'00:02:00','x3 serie al multipower');";
					command2.ExecuteNonQuery();
					command2.CommandText = "INSERT INTO Associazione VALUES(" + idScheda.ToString() + ",4,10,'00:01:30','x3 10 per gamba');";
					command2.ExecuteNonQuery();
					command2.CommandText = "INSERT INTO Assegnazione VALUES (NULL,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',1,'"+cod_fiscale_cliente+"','"+cod_fiscale_trainer+"',"+idScheda.ToString()+");";
					command2.ExecuteNonQuery();
					transaction.Commit();
					connection.Close();
					return true;
				}
				catch (Exception ex)
				{
					// Attempt to roll back the transaction.
					try
					{
						transaction.Rollback();
						return false;
					}
					catch (Exception ex2)
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}

	/// METODO CHE TORNA TRUE SE LA SHEDA ATTUALE DELL'UTENTE è SCADUTA, O SE NON HA ALCINA SCHEDA ATTIVA
	public bool ControlloSchedaScaduta(string cod_cliente)
	{
		DateTime dataAssegnazioneScheda = DateTime.Now;
		int durata = -1;
		using (MySqlConnection connection = new MySqlConnection(stringConnection))
		{
			connection.Open();
			try
			{
				string query = "SELECT Assegnazione.data_assegnazione,Scheda.durata FROM Scheda JOIN Assegnazione ON(Assegnazione.id_scheda = Scheda.id) WHERE Assegnazione.inUso = 1 AND Assegnazione.cod_cliente = '" + cod_cliente + "' ORDER BY Assegnazione.data_assegnazione  DESC";
				MySqlCommand command = new MySqlCommand(query, connection);
				using (MySqlDataReader reader = command.ExecuteReader()) ///USATO PER LEGGERE UN FLUSSO DI DATI (SELECT)
				{
					if (!reader.HasRows) ///signifa che il cliente non ha schede attive
						return true;
					while(reader.Read())
					{
						dataAssegnazioneScheda = reader.GetDateTime("data_assegnazione");
						durata = reader.GetInt32("durata");
						break; ///ci servono solo i valori della prima riga, che avendo messo l'order by,
							   ///sarà la riga che contiene la scheda in uso più recente
					}
				}
				connection.Close();
				if (dataAssegnazioneScheda.AddMonths(durata).CompareTo(DateTime.Now) < 0) //se sommando i mesi 
				                    //della durata della scheda alla data alla quale questa è stata iniziata,
										//la data ottenuta è minore della data attuale, allore la scheda è scaduta
					return true;
				return false;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}
