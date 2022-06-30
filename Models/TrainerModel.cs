using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class TrainerModel : PersonaModel
    {

        public TrainerModel(int idTrainer, string nome, string cognome, string email, DateTime data_nascita, string telefono, definizioneSesso sesso) : base(idTrainer, nome, cognome, email, data_nascita, telefono, sesso)
        {

        }

    }
}
