using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class EsercizioModel
    {
        int id;
        string descrizione;
        string immagine;

        public EsercizioModel(int id, string descrizione, string immagine)
        {
            this.id = id;
            this.descrizione = descrizione;
            this.immagine = immagine;
        }

        public int Id { get => id; set => id = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string Immagine { get => immagine; set => immagine = value; }
    }
}