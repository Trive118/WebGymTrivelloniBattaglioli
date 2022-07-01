using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class TrainerModel : PersonaModel
    {

        public TrainerModel(int idTrainer, string nome, string cognome, string email, DateTime data_nascita, string telefono,string password, string sesso) : base(idTrainer, nome, cognome, email, data_nascita, telefono,password, sesso)
        {

        }

        public TrainerModel(string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso) : base(nome, cognome, email, data_nascita, telefono, password, sesso)
        {

        }

    }
}
