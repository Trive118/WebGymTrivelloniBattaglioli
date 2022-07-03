﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;
using WebGymTrivelloniBattaglioli.Models;
using WebGymTrivelloniBattaglioli.ServiceReferenceWCF;

namespace WebGymTrivelloniBattaglioli.Controllers
{
    public class HomeController : Controller
    {
        static ClienteModel loggedClient;
        ServiceClient wcfClient = new ServiceReferenceWCF.ServiceClient();
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

        //VIENE LANCIATO QUANDO L'UTENTE COMPILA IL FORM DI REGISTRAZIONE CON I PROPRI DATI
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
                ///VALE 0 SE ABBIAMO SCLETO IL PRIMO CAMPO DEL MENU' A TENDINA, CIOE' 'M', OPPURE VALE
                ///1 SE ABBIAMO SCELTO IL SECONDO ELEMENTO DEL MENU' A TENDINA CIOE' 'F'.
                string sesso = Request["Sesso"];
                if (sesso == "0")
                    sesso = "M";
                else
                    sesso = "F";
                ///MessageBox.Show(sesso); DEBUG, MOSTRA FINESTRA DI WINDOWS CON OUTPUT
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
            datasql = date.Year.ToString()+"-"+ date.Month.ToString() + "-" + date.Day.ToString() + " " + date.Hour + ":" + date.Minute + ":" + date.Second + ":";
            return datasql;
        }

        //VIENE LANCIATO QUANDO L'UTENTE CONFERMA I PROPRI DATI NELLA SCHERMATA RIASSUNTIVA, E PERMETTE
        //DI CARICARE LA PAGINA SUCCESSIVA PER LA SCELTA DEL CONTRATTO
        public ActionResult InserisciClientedb()
        {
            string sesso;
            if (loggedClient.Sesso == definizioneSesso.M)
                sesso = "M";
            else
                sesso = "F";
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

        //VIENE LANCIATO QUANDO L'UTENTE HA SCELTO UNO TRA I POSSIBILI CONTRATTI PER ISCRIVERSI, E 
        //QUINDI SI OCCUPA DI REGISTRARE L'ASSOCIAZIONE TRA CLIENTE E CONTRATTO, E PORTA IL CLIENTE AD 
        //UNA SCHERMATA CHE CONFERMI L'AVVENUTA REGISTRAZIONE CON SUCCESSO
        [HttpPost]
        public ActionResult SceltaContrattoUtente()
        {
            int id = Convert.ToInt32(Request["Id"]);
            string data_iscrizione = generateStringByDateForMySql(DateTime.Now);
            wcfClient.AddContractToClient(id, loggedClient.Codice_fiscale, data_iscrizione);
            return View();
        }
    }
}