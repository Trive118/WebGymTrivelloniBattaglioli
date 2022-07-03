using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGymTrivelloniBattaglioli.Models;
using WebGymTrivelloniBattaglioli.ServiceReferenceWCF;

namespace WebGymTrivelloniBattaglioli.Controllers
{
    public class HomeController : Controller
    {
        static ClienteModel loggedClient;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrati()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UtenteView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HandleConfirmClick()
        {
            if (ModelState.IsValid)  ///controllo se il format è stato compilato correttamente
            { 
                string codice_fiscale = Request["Codice_fiscale"];
                string nome = Request["Nome"];
                string cognome = Request["Cognome"];
                string mail = Request["Email"];
                string password = Request["Password"];
                DateTime data_nascita = Convert.ToDateTime(Request["Data_nascita"]);
                string sesso = Request["Sesso"];
                string telefono = Request["Telefono"];
                loggedClient = new ClienteModel(codice_fiscale, nome, cognome, mail, data_nascita, telefono, password, sesso);
                return View("ConfirmUserDataView",loggedClient);  ///Lancio la vista ConfirmUserDataView, passando come oggetto model
                                                                  ///da visulizzare i dati contenuti nell'oggetto loggedClient
            }
            return null;
        }

        private string generateStringByDateForMySql(DateTime date)
        {
            string datasql;
            datasql = date.Year.ToString()+"-"+ date.Month.ToString() + "-" + date.Day.ToString();
            return datasql;
        }
        public ActionResult InserisciClientedb()
        {
            string sesso;
            if (loggedClient.Sesso == definizioneSesso.M)
                sesso = "M";
            else
                sesso = "F";
            var wcfClient = new ServiceReferenceWCF.ServiceClient();
            wcfClient.InserisciCliente(loggedClient.Codice_fiscale,loggedClient.Nome, loggedClient.Cognome, loggedClient.Email,generateStringByDateForMySql(loggedClient.Data_nascita) , loggedClient.Telefono, loggedClient.Password, sesso);
            ///FINE DELL'INSERIMENTO DEI DATI DELL'UTENTE NEL DATABSE
            ///------------------------------------------------------
            ///DOWLOAD DI TUTTI I CONTRATTI DISPONIBILI DA MOSTRARE ALL'UTENTE NELLA SEZIONE SUCCESSIVA
            List<ContractDTO> contracts_generics = wcfClient.GetAvailableContracts().ToList();
            List<ContrattoModel> contracts = new List<ContrattoModel>();
            foreach(ContractDTO contract in contracts_generics)
                contracts.Add(new ContrattoModel(contract.id, contract.descrizione, contract.prezzo, contract.durata));
            return View("ChooseContract",contracts);
        }
    }
}