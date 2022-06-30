using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class IscrizioneModel
    {
        DateTime data_inizio;
        DateTime data_fine;  /// più comodo si può ricavare dai dati quando vengono recuperati dal db
        string motivo;
        ContrattoModel contratto;

        public IscrizioneModel(DateTime data_inizio, string motivo, DateTime data_fine, ContrattoModel contratto)
        {
            this.data_inizio = data_inizio;
            this.motivo = motivo;
            this.data_fine = data_fine;
            this.contratto = contratto;
        }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire la data di inizio iscrizione!")]
        [Display(Name = "Data inizio iscrizione")]
        [DataType(DataType.Date)]
        public DateTime Data_inizio { get => data_inizio; set => data_inizio = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il motivo dell'iscrizione!")]
        [Display(Name = "Motivo iscrizione")]
        public string Motivo { get => motivo; set => motivo = value; }
        public DateTime Data_fine { get => data_fine; set => data_fine = value; }
        public ContrattoModel Contratto { get => contratto; set => contratto = value; }
    }
}