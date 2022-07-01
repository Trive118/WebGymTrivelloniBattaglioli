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
        string password;
        definizioneSesso sesso;

        protected PersonaModel(int id, string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso)
        {
            this.id = id;
            this.nome = nome;
            this.cognome = cognome;
            this.email = email;
            this.data_nascita = data_nascita;
            this.telefono = telefono;
            this.password = password;
            if(sesso == "M")
                this.sesso = definizioneSesso.M;
            else
                this.sesso = definizioneSesso.F;
        }

        protected PersonaModel( string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.email = email;
            this.data_nascita = data_nascita;
            this.telefono = telefono;
            this.password = password;
            if (sesso == "M")
                this.sesso = definizioneSesso.M;
            else
                this.sesso = definizioneSesso.F;
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
        [Required(ErrorMessage = "La mail non può essere lasciata vuota!!")]
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

        [DataMember]
        [Required(ErrorMessage = "Devi immettere una password")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,25}$", ErrorMessage = "La password deve contenere almeno 8 caratteri di cui almeno un numero, un carattere maiuscolo e uno minuscolo!!")]
        [Display(Name = "Password")]
        ///The conditions are string must be between 8 and 15 characters long. 
        ///string must contain at least one number. string must contain at least one uppercase letter. 
        ///string must contain at least one lowercase letter.
        public string Password { get => password; set => password = value; }

        ///Nell'html della pagina per l'inserimento:<div class="col-md-10">  
        //  @Html.EnumDropDownListFor(model => model.Sex)  
        //@Html.ValidationMessageFor(model => model.Sex)  
        //</div>
    }
}