using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

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

        [DataMember]
        [Required(ErrorMessage = "Devi inserire la descrizione del contratto!")]
        [Display(Name = "Descrizione contratto")]
        public string Descrizione { get => descrizione; set => descrizione = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire un prezzo!")]
        [Display(Name = "Prezzo per pagamento contratto")]
        public double Prezzo { get => prezzo; set => prezzo = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire una durata del contratto!")]
        [Display(Name = "Durata contratto")]
        [DataType(DataType.Duration)]
        public int Durata { get => durata; set => durata = value; }

        
    }
}