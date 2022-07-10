using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class TrainerModel : PersonaModel
    {

        public TrainerModel(string codice_fiscale, string nome, string cognome, string email, DateTime data_nascita, string telefono,string password, string sesso) : base(codice_fiscale, nome, cognome, email, data_nascita, telefono,password, sesso)
        {

        }

        public TrainerModel(string codice_fiscale, string nome, string cognome) : base(codice_fiscale, nome, cognome)
        {

        }
    }
}
