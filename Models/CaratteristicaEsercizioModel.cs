using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// LA CLASSE CaratteristicaEsercizioModel VIENE USATA COME SOSTITUTO DELLA TABELLA 'relazione' DEL DATABASE,
/// IN MODO CHE AD OGNI SCHEDA SIA ASSOCIATA DIRETTAMENTE UNA LISTA DI ESERCIZI, ACCOMPAGNATA DA UN'ALTRA LISTA CHE
/// CONTIENE PER OGNI ESERCIZIO LA SUA DESCRIZIONE PER L'USO IN QUELLA SPECIFICA SCHEDA
/// </summary>
namespace WebGymTrivelloniBattaglioli.Models
{
    public class CaratteristicaEsercizioModel
    {
        int numero_ripetizioni;
        TimeSpan recupero;
        string commento;

        public CaratteristicaEsercizioModel(int numero_ripetizioni, TimeSpan recupero, string commento)
        {
            this.numero_ripetizioni = numero_ripetizioni;
            this.recupero = recupero;
            this.commento = commento;
        }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il numero di ripetizioni!")]
        [Display(Name = "Numero ripetizioni")]
        public int Numero_ripetizioni { get => numero_ripetizioni; set => numero_ripetizioni = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire un tempo di recupero!")]
        [Display(Name = "Tempo di recupero")]
        [DataType(DataType.Duration)]
        public TimeSpan Recupero { get => recupero; set => recupero = value; }

        [DataMember]
        [Display(Name = "Note sull'esercizio")]
        public string Commento { get => commento; set => commento = value; }
    }
}