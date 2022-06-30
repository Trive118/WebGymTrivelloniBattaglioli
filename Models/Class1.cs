using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class Persona
    {
        int id;
        string nome;
        string cognome;
        string email;
        string data_nascita;
        string telefono;
        char sesso;
        string ruolo;

        public Persona(int id, string nome, string cognome, string email, string data_nascita, string telefono, char sesso, string ruolo)
        {
            this.id = id;
            this.nome = nome;
            this.cognome = cognome;
            this.email = email;
            this.data_nascita = data_nascita;
            this.telefono = telefono;
            this.sesso = sesso;
            this.ruolo = ruolo;
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Email { get => email; set => email = value; }
        public string Data_nascita { get => data_nascita; set => data_nascita = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public char Sesso { get => sesso; set => sesso = value; }
        public string Ruolo { get => ruolo; set => ruolo = value; }
    }
}