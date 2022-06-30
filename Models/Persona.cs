using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public enum definizioneSesso {M,F};
    public abstract class Persona
    {
        int id;
        string nome;
        string cognome;
        string email;
        DateTime data_nascita;
        string telefono;
        definizioneSesso sesso;

        protected Persona(int id, string nome, string cognome, string email, DateTime data_nascita, string telefono, definizioneSesso sesso)
        {
            this.id = id;
            this.nome = nome;
            this.cognome = cognome;
            this.email = email;
            this.data_nascita = data_nascita;
            this.telefono = telefono;
            this.sesso = sesso;
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Email { get => email; set => email = value; }
        public DateTime Data_nascita { get => data_nascita; set => data_nascita = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public definizioneSesso Sesso { get => sesso; set => sesso = value; }
    }
}