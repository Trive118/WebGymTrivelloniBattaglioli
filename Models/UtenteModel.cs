using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplicationPrimoMVCInserimentUjtente.Models
{
    [Serializable]
    [DataContract()]
    //mAIRO ASGFSDFGDFG
    public class UtenteModel
    {
        [DataMember]
        [Required(ErrorMessage = "Devi inserire il nome")] 
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Devi inserire il cognome")]
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Devi inserire la data di nascita")]
        [Display(Name = "Data di nascita")]
        [DataType(DataType.Date)]
        public DateTime Data_nascita { get; set; }

        [Display(Name = "Luogo di residenza")]
        public string Residenza { get; set; }

        [Required(ErrorMessage = "Devi inserire la password")]
        [Display(Name = "password")]
        [StringLength(8, ErrorMessage = "La password deve avere 8 caratteri!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}