using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

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

        public EsercizioModel(string descrizione, string immagine)
        {
            this.id = -1;
            this.descrizione = descrizione;
            this.immagine = immagine;
        }

        public int Id { get => id; set => id = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire una descrizione!")]
        [Display(Name = "Descrizione esercizio")]
        public string Descrizione { get => descrizione; set => descrizione = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il link all'immagine che vuoi inserire!")]
        [Display(Name = "Link all'immagine da inserire")]
        public string Immagine { get => immagine; set => immagine = value; }
    }
}