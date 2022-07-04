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
	CompositeType GetDataUsingDataContract(CompositeType composite);

	[OperationContract]
	bool InserisciCliente(string codice_fiscale,string nome,string cognome,string mail, string data_nascita, string telefono, string password, string sesso);
	
	[OperationContract]
	List<ContractDTO> GetAvailableContracts();

	[OperationContract]
	bool AddContractToClient(int id,string codice_fiscale,string startDate);

	/// METODO CHE TORNA TRUE SE L'UTENTE E' GIA' REGISTRATO AL SITO
	[OperationContract]
	bool AlreadyRegistered(string codice_fiscale,string mail);

	[OperationContract]
	bool ConvalidLogIn(string mail,string password);
	
	[OperationContract]
	int CercaPersonalTrainerNelDB(string email, string password);
}

// Per aggiungere tipi compositi alle operazioni del servizio utilizzare un contratto di dati come descritto nell'esempio seguente.
[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

	[DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}

[DataContract]
public class ContractDTO
{
	public ContractDTO(int id, string descrizione, double prezzo, int durata)
	{
		this.id = id;
		this.descrizione = descrizione;
		this.prezzo = prezzo;
		this.durata = durata;
	}

	[DataMember]
	public int id { get; set; }
	[DataMember]
	public string descrizione { get; set; }
	[DataMember]
	public double prezzo { get; set; }
	[DataMember]
	public int durata { get; set; }
}

