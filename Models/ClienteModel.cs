using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebGymTrivelloniBattaglioli.Models
{
    public class ClienteModel : Persona
    {
        
        public ClienteModel(int idCliente, string nome, string cognome, string email, DateTime data_nascita, string telefono, definizioneSesso sesso) : base(idCliente, nome, cognome, email, data_nascita, telefono, sesso)
        {

        }

    }
}