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
        string codice_fiscale;
        string nome;
        string cognome;
        string email;
        DateTime data_nascita;
        string telefono;
        string password;
        definizioneSesso sesso;

        protected PersonaModel(string codice_fiscale, string nome, string cognome, string email, DateTime data_nascita, string telefono, string password, string sesso)
        {
            this.codice_fiscale = codice_fiscale;
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

        [DataMember]
        [Required(ErrorMessage = "Devi immettere il tuo codice fiscale!")]
        //[RegularExpression(@"/^(?:[B-DF-HJ-NP-TV-Z](?:[AEIOU]{2}|[AEIOU]X)|[AEIOU]{2}X|[B-DF-HJ-NP-TV-Z]{2}[A-Z]){2}[\dLMNP-V]{2}(?:[A-EHLMPR-T](?:[04LQ][1-9MNP-V]|[1256LMRS][\dLMNP-V])|[DHPS][37PT][0L]|[ACELMRT][37PT][01LM])(?:[A-MZ][1-9MNP-V][\dLMNP-V]{2}|[A-M][0L](?:[\dLMNP-V][1-9MNP-V]|[1-9MNP-V][0L]))[A-Z]$/i", ErrorMessage = "Formato codice fiscale non valido!!")]
        [Display(Name = "Codice fiscale")]
        public string Codice_fiscale { get => codice_fiscale; set => codice_fiscale = value; }
    }
}