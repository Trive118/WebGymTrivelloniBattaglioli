using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebGymTrivelloniBattaglioli.Models
{
    public class ClienteModel : PersonaModel
    {
        List<IscrizioneModel> iscrizioni;

        public ClienteModel(int idCliente, string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso,List<IscrizioneModel> iscrizioni) : base(idCliente, nome, cognome, email, data_nascita, telefono,password, sesso)
        {
            this.iscrizioni = iscrizioni;
        }

        ///Lista delle iscrizioni effettuate dal cliente presso la nostra struttura

        public ClienteModel(int idCliente, string nome, string cognome, string email, DateTime data_nascita, string telefono,string password, string sesso) : base(idCliente, nome, cognome, email, data_nascita, telefono,password, sesso)
        {

        }

        public ClienteModel(string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso) : base(nome, cognome, email, data_nascita, telefono, password, sesso)
        {

        }

        public void newIscrizione(ContrattoModel contratto,DateTime data_inizio,string motivazione)
        {
            ///inserimento nel database della nuova iscrizione
        }

    }
}