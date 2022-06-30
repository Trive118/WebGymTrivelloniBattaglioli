using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public DateTime Data_inizio { get => data_inizio; set => data_inizio = value; }
        public string Motivo { get => motivo; set => motivo = value; }
        public DateTime Data_fine { get => data_fine; set => data_fine = value; }
        public ContrattoModel Contratto { get => contratto; set => contratto = value; }
    }
}