using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebGymTrivelloniBattaglioli.Models
{
    public enum definizioneSesso {M,F};

    [Serializable]
    [DataContract()]
    public abstract class PersonaModel
    {
        int id;
        string nome;
        string cognome;
        string email;
        DateTime data_nascita;
        string telefono;
        definizioneSesso sesso;

        protected PersonaModel(int id, string nome, string cognome, string email, DateTime data_nascita, string telefono, definizioneSesso sesso)
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

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il nome")]
        [Display(Name = "Nome")]
        public string Nome { get => nome; set => nome = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire il cognome")]
        [Display(Name = "Cognome")]
        public string Cognome { get => cognome; set => cognome = value; }

        [DataMember]
        [Required(ErrorMessage = "Field can't be empty")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Mail field not valid!!")]
        public string Email { get => email; set => email = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire la data di nascita")]
        [Display(Name = "Data di nascita")]
        [DataType(DataType.Date)]
        public DateTime Data_nascita { get => data_nascita; set => data_nascita = value; }

        [DataMember]
        [Display(Name = "Numero di telefono")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get => telefono; set => telefono = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi scegliere un sesso")]
        [Display(Name = "Sesso")]
        public definizioneSesso Sesso { get => sesso; set => sesso = value; }

        ///Nell'html della pagina per l'inserimento:<div class="col-md-10">  
      //  @Html.EnumDropDownListFor(model => model.Sex)  
     //@Html.ValidationMessageFor(model => model.Sex)  
        //</div>
    }
}