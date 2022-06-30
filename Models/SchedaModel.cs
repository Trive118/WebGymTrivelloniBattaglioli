using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace WebGymTrivelloniBattaglioli.Models
{
    public class SchedaModel
    {
        int id;
        string titolo;
        int durata; ///in mesi
        TrainerModel trainer;  /// creatore della scheda
        ClienteModel cliente;   /// cliente che sta usufruendo di quella scheda
        List<EsercizioModel> esercizi;   //lista di esercizi contenuti nella scheda
        List<CaratteristicaEsercizioModel> caratteristiche_esercizi;  //all'i-esima posizione della lista 'caratteristiche_esercizi'
                                                                      //si trovano le caratteristiche che descrivono l'i-esimo
                                                                      //esercizio della lista 'esercizi'

        public SchedaModel(int id, string titolo, int durata, TrainerModel trainer, ClienteModel cliente)
        {
            this.id = id;
            this.titolo = titolo;
            this.durata = durata;
            this.trainer = trainer;
            this.cliente = cliente;
        }

        public int Id { get => id; set => id = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il titolo alla scheda!")]
        [Display(Name = "Titolo scheda")]
        public string Titolo { get => titolo; set => titolo = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire la durata della scheda!")]
        [Display(Name = "Durata (in mesi) della scheda")]
        public int Durata { get => durata; set => durata = value; }
        public TrainerModel Trainer { get => trainer; set => trainer = value; }
        public ClienteModel Cliente { get => cliente; set => cliente = value; }
    }
}