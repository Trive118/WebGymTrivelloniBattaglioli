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

        protected PersonaModel(string codice_fiscale, string nome, string cognome)
        {
            this.codice_fiscale = codice_fiscale;
            this.nome = nome;
            this.cognome = cognome;
        }

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
        //RegualExpression che verifica la correttezza dell'indirizzo email inserito
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Formato mail non corretto!!")]
        public string Email { get => email; set => email = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi inserire la data di nascita")]
        [Display(Name = "Data di nascita")]
        [DataType(DataType.Date)]
        public DateTime Data_nascita { get => data_nascita; set => data_nascita = value; }

        [DataMember]
        [Display(Name = "Numero di telefono")]
        [MaxLength(10, ErrorMessage = "Il numero di telefono e' composto da 10 numeri!")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get => telefono; set => telefono = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi scegliere un sesso")]
        [Display(Name = "Sesso")]
        public definizioneSesso Sesso { get => sesso; set => sesso = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi immettere una password")]
        [DataType(DataType.Password)]
        //RegularExpression che verifica la correttezza della password inserita. Almeno 8 caratteri di lunghezza con almeno un carattere maiuscolo.
        ///(?=.*\d) significa che deve esserci almeno un carattere che rispetta la regola che lo precede
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,25}$", ErrorMessage = "La password deve contenere almeno 8 caratteri di cui almeno un numero, un carattere maiuscolo e uno minuscolo!!")]
        [Display(Name = "Password")]
        ///The conditions are string must be between 8 and 15 characters long. 
        ///string must contain at least one number. string must contain at least one uppercase letter. 
        ///string must contain at least one lowercase letter.
        public string Password { get => password; set => password = value; }

        [DataMember]
        [Required(ErrorMessage = "Devi immettere il tuo codice fiscale!")]
        //RegularExpression che verifica la correttezza del codice fiscale: si controlla che lettere e cifre siano ai posti giusti. Per il resto, la scelta di lettere e caratteri può essere totalmente casuale
        [RegularExpression(@"^([A-Z]{6}[0-9LMNPQRSTUV]{2}[ABCDEHLMPRST]{1}[0-9LMNPQRSTUV]{2}[A-Z]{1}[0-9LMNPQRSTUV]{3}[A-Z]{1})$|([0-9]{11})$", ErrorMessage = "Formato codice fiscale non valido!!")]
        [MaxLength(16,ErrorMessage = "Il codice fiscale deve avere 16 caratteri!!")]
        [Display(Name = "Codice fiscale")]
        public string Codice_fiscale { get => codice_fiscale; set => codice_fiscale = value; }
    }
}