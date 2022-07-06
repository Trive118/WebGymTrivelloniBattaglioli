using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IService" nel codice e nel file di configurazione contemporaneamente.
[ServiceContract]
public interface IService
{
	[OperationContract]
	bool InserisciCliente(string codice_fiscale,string nome,string cognome,string mail, string data_nascita, string telefono, string password, string sesso);
	
	[OperationContract]
	List<ContractDTO> GetAvailableContracts();

	[OperationContract]
	bool AddContractToClient(int id,string codice_fiscale,string startDate);

	[OperationContract]
	bool CercaPersonalTrainerNelDB(string email, string password);

	/// METODO CHE TORNA TRUE SE L'UTENTE E' GIA' REGISTRATO AL SITO
	[OperationContract]
	bool AlreadyRegistered(string codice_fiscale,string mail);

	[OperationContract]
	bool ConvalidLogIn(string mail,string password);

	[OperationContract]
	UserDTO GetUserByEmail(string mail);

	[OperationContract]
	ContractDTO GetUserActiveContract(string codice_fiscale);

	[OperationContract]
	List<UserDTO> CercaClientiAssociatiAlTrainer(string cod_fiscale_trainer);

	[OperationContract]
	List<SchedaDTO> GetSchedeUtente(string cod_fiscale);

	[OperationContract]
	bool InserisciSchedaNelDB(string titolo, int durata);

	[OperationContract]
	bool AggiornaUtilizzoSchede(string cod_fiscale_trainer, string cod_fiscale_cliente);

	[OperationContract]
	int OttieniIdUtimaSchedaInserita();

	[OperationContract]
	bool AggiungiNuovaAssegnazione(string cod_fiscale_trainer, string cod_fiscale_cliente, int idScheda,string data);

	[OperationContract]
	bool AddNewExerciseToCardGym(int id_scheda, string descrizione, int num_ripetizioni, TimeSpan tempo_recupero, string commento, string immagine);

	[OperationContract]
	bool OttieniEserciziAssociatiAllaScheda(int id_scheda,ref List<EsercizioDTO> lista_esercizi,ref List<CaratteristicaEsercizioDTO> lista_caratteristiche);
}

[DataContract]
public class SchedaDTO
{
    public SchedaDTO(int id, string titolo, int durata, string mail_cliente, string mail_trainer, bool in_uso, List<EsercizioDTO> esercizi, List<CaratteristicaEsercizioDTO> caratteristica_esercizi)
    {
        Id = id;
        Titolo = titolo;
		Durata = durata;
        Mail_cliente = mail_cliente;
        Mail_trainer = mail_trainer;
        In_uso = in_uso;
        Esercizi = esercizi;
        Caratteristica_esercizi = caratteristica_esercizi;
    }

	public SchedaDTO(int id, string titolo, int durata, DateTime data_inizio, string mail_cliente, string mail_trainer, bool in_uso, List<EsercizioDTO> esercizi, List<CaratteristicaEsercizioDTO> caratteristica_esercizi)
	{
		Id = id;
		Titolo = titolo;
		Durata = durata;
		Data_inizio = data_inizio;
		Mail_cliente = mail_cliente;
		Mail_trainer = mail_trainer;
		In_uso = in_uso;
		Esercizi = esercizi;
		Caratteristica_esercizi = caratteristica_esercizi;
	}

	public SchedaDTO()
    {
    }

	[DataMember]
	public int Id { get; set; }
	[DataMember]
	public string Titolo { get; set; }
	[DataMember]
	public DateTime Data_inizio { get; set; }
	[DataMember]
	public int Durata { get; set; }
	[DataMember]
	public string Mail_cliente { get; set; }
	[DataMember]
	public string Mail_trainer { get; set; }
	[DataMember]
	public bool In_uso { get; set; }
	[DataMember]
	public List<EsercizioDTO> Esercizi{ get; set; }
	[DataMember]
	public List<CaratteristicaEsercizioDTO> Caratteristica_esercizi { get; set; }
}

[DataContract]
public class EsercizioDTO
{
    public EsercizioDTO(string descrizione, string immagine)
    {
        Descrizione = descrizione;
        Immagine = immagine;
    }

	[DataMember]
	public string Descrizione { get; set; }
	[DataMember]
	public string Immagine { get; set; }
}

[DataContract]
public class CaratteristicaEsercizioDTO
{
    public CaratteristicaEsercizioDTO(int num_ripetizioni, TimeSpan recupero, string commento)
    {
        Num_ripetizioni = num_ripetizioni;
        Recupero = recupero;
        Commento = commento;
    }

    [DataMember]
	public int Num_ripetizioni { get; set; }
	[DataMember]
	public TimeSpan Recupero { get; set; }
	[DataMember]
	public string Commento { get; set; }
}

[DataContract]
public class ContractDTO
{
	public ContractDTO(int id, string descrizione, double prezzo, int durata)
	{
		this.Id = id;
		this.Descrizione = descrizione;
		this.Prezzo = prezzo;
		this.Durata = durata;
	}

    public ContractDTO(int id, string descrizione, double prezzo, int durata, DateTime data_inizio)
    {
        this.Id = id;
        this.Descrizione = descrizione;
        this.Prezzo = prezzo;
        this.Durata = durata;
        this.Data_inizio = data_inizio;
    }

    [DataMember]
	public int Id { get; set; }
	[DataMember]
	public string Descrizione { get; set; }
	[DataMember]
	public double Prezzo { get; set; }
	[DataMember]
	public int Durata { get; set; }
	[DataMember]
	public DateTime Data_inizio { get; set; }
}

[DataContract]
public class UserDTO
{
    public UserDTO(string codice_fiscale, string nome, string cognome, string mail, DateTime data_nascita, string telefono, string sesso, string ruolo, string password)
    {
        this.codice_fiscale = codice_fiscale;
        this.nome = nome;
        this.cognome = cognome;
        this.mail = mail;
        this.data_nascita = data_nascita;
        this.telefono = telefono;
        this.sesso = sesso;
        this.ruolo = ruolo;
        this.password = password;
    }

	public UserDTO()
	{
		
	}

	[DataMember]
	public string codice_fiscale { get; set; }
	[DataMember]
	public string nome { get; set; }
	[DataMember]
	public string cognome { get; set; }
	[DataMember]
	public string mail { get; set; }
	[DataMember]
	public DateTime data_nascita { get; set; }
	[DataMember]
	public string telefono { get; set; }
	[DataMember]
	public string sesso { get; set; }
	[DataMember]
	public string ruolo { get; set; }
	[DataMember]
	public string password { get; set; }
}

