using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class ContrattoModel
    {
        int id;
        string descrizione;
        double prezzo;
        int durata;

        public ContrattoModel(int id, string descrizione, double prezzo, int durata)
        {
            this.id = id;
            this.descrizione = descrizione;
            this.prezzo = prezzo;
            this.durata = durata; ///in mesi
        }

        public int Id { get => id; set => id = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public double Prezzo { get => prezzo; set => prezzo = value; }
        public int Durata { get => durata; set => durata = value; }

        
    }
}