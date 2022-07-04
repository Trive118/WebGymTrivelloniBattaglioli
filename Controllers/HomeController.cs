using System;
using System.Collections.Generic;
using System.Dynamic;
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
        static TrainerModel loggedTrainer;
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

        public ActionResult LogIn()
        {
            return View();
        }

        //VIENE LANCIATO QUANDO L'UTENTE COMPILA IL FORM DI REGISTRAZIONE CON I PROPRI DATI
        [HttpPost]
        public ActionResult HandleConfirmClick()
        {
            if (ModelState.IsValid)  ///controllo se il format è stato compilato correttamente
            {
                try
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
                    if (!wcfClient.AlreadyRegistered(codice_fiscale,mail))
                    {
                        loggedClient = new ClienteModel(codice_fiscale, nome, cognome, mail, data_nascita, telefono, password, sesso);
                        return View("ConfirmUserDataView", loggedClient);  ///Lancio la vista ConfirmUserDataView, passando come oggetto model
                                                                           ///da visulizzare i dati contenuti nell'oggetto loggedClient
                    }
                    return View("GiaRegistrato");
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("ErrorPage");
                }
            }
            return View("ErrorPage"); //TRIVE SEI SICURO CHE VADA BENE QUI QUEL RETURN?? :/
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
            try
            {
                bool success = wcfClient.InserisciCliente(loggedClient.Codice_fiscale, loggedClient.Nome, loggedClient.Cognome, loggedClient.Email, generateStringByDateForMySql(loggedClient.Data_nascita), loggedClient.Telefono, loggedClient.Password, sesso);
                if(!success)
                {
                    ViewData["ErrorMessage"] = "Errore nell'inserimento dei tuoi dati nel server, la preghiamo di riprovare!";
                    return View("ErrorPage");
                }
                ///FINE DELL'INSERIMENTO DEI DATI DELL'UTENTE NEL DATABSE
                ///------------------------------------------------------
                ///DOWLOAD DI TUTTI I CONTRATTI DISPONIBILI DA MOSTRARE ALL'UTENTE NELLA SEZIONE SUCCESSIVA
                List<ContractDTO> contracts_generics = wcfClient.GetAvailableContracts().ToList();
                List<ContrattoModel> contracts = new List<ContrattoModel>();
                foreach (ContractDTO contract in contracts_generics)
                    contracts.Add(new ContrattoModel(contract.Id, contract.Descrizione, contract.Prezzo, contract.Durata));
                return View("ChooseContract", contracts);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        //VIENE LANCIATO QUANDO L'UTENTE HA SCELTO UNO TRA I POSSIBILI CONTRATTI PER ISCRIVERSI, E 
        //QUINDI SI OCCUPA DI REGISTRARE L'ASSOCIAZIONE TRA CLIENTE E CONTRATTO, E PORTA IL CLIENTE AD 
        //UNA SCHERMATA CHE CONFERMI L'AVVENUTA REGISTRAZIONE CON SUCCESSO
        [HttpPost]
        public ActionResult SceltaContrattoUtente()
        {
            try
            {
                int id = Convert.ToInt32(Request["Id"]);
                string data_iscrizione = generateStringByDateForMySql(DateTime.Now);
                bool succes = wcfClient.AddContractToClient(id, loggedClient.Codice_fiscale, data_iscrizione);
                if(!succes)
                {
                    ViewData["ErrorMessage"] = "Errore nella selezione del contratto, la preghiamo di tornare indietro e riprovare!";
                    return View("ErrorPage");
                }
                return View();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        /// VIENE RICHIAMATO QUANDO L'UTENTE IMMETTE LE SUE CREDENZIALI NEL FORM DI LOGIN.
        /// INTERROGA IL DATABASE TRAMITE WCF E SE L'UTENTE SI è LOGGATO CORRETTAMENTE GLI PERMETTE
        /// DI ENTRARE NELLA SUA AREA PERSONALE, ALTRIMENTI VISUALIZZA UNA PAGINA DI ERRORE
        [HttpPost]
        public ActionResult EffettuaLogin()
        {
            try
            {
                string mail = Request["Email"];
                string password = Request["Password"];
                if (wcfClient.ConvalidLogIn(mail, password))
                {
                    UserDTO user = wcfClient.GetUserByEmail(mail); //trova la persona con la mail passata per argomento
                    loggedClient = new ClienteModel(user.codice_fiscale,user.nome,user.cognome,user.mail,user.data_nascita,user.telefono,user.password,user.sesso); //istanziato oggetto
                    ContractDTO c = wcfClient.GetUserActiveContract(loggedClient.Codice_fiscale);
                    ContrattoModel activeContract = new ContrattoModel(c.Id, c.Descrizione, c.Prezzo, c.Durata);
                    int giorni_scadenza = (c.Data_inizio.AddMonths(activeContract.Durata) - DateTime.Now).Days;
                    dynamic mymodel = new ExpandoObject();
                    mymodel.Cliente = loggedClient;
                    mymodel.Contratto = activeContract;
                    mymodel.GiorniScadenza = giorni_scadenza;
                    mymodel.DataInizio = c.Data_inizio;
                    mymodel.DataScadenza = c.Data_inizio.AddMonths(c.Durata);
                    return View("MainPageClient", mymodel);
                }
                return View("DatiErratiLogin");
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        /// VIENE RICHIAMATO QUANDO L'UTENTE DALLA SUA AREA PERSONALE VUOLE VISUALIZZARE LA PROPRIA ATTUALE
        /// SCHEDA DI ALLENAMENTO
        [HttpPost]
        public ActionResult ShowScheda()
        {
            try
            {
                SchedaDTO schedaAttivaDTO = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList().FirstOrDefault(scheda => scheda.In_uso == true); ///torna la prima scheda che viene trovata come attiva
                UserDTO trainerDTO = wcfClient.GetUserByEmail(schedaAttivaDTO.Mail_trainer);
                TrainerModel trainer = new TrainerModel(trainerDTO.codice_fiscale,trainerDTO.nome,trainerDTO.cognome,trainerDTO.mail,trainerDTO.data_nascita,trainerDTO.telefono,trainerDTO.password,trainerDTO.sesso);
                SchedaModel scheda_attiva = new SchedaModel(schedaAttivaDTO.Id, schedaAttivaDTO.Titolo, schedaAttivaDTO.Durata, schedaAttivaDTO.In_uso, trainer, loggedClient,new List<EsercizioModel>(),new List<CaratteristicaEsercizioModel>());
                foreach(EsercizioDTO ese in schedaAttivaDTO.Esercizi)
                    scheda_attiva.Esercizi.Add(new EsercizioModel(ese.Descrizione, ese.Immagine));
                foreach (CaratteristicaEsercizioDTO cara in schedaAttivaDTO.Caratteristica_esercizi)
                    scheda_attiva.Caratteristiche_esercizi.Add(new CaratteristicaEsercizioModel(cara.Num_ripetizioni, cara.Recupero, cara.Commento));
                return View(scheda_attiva);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        /// METODO PER CONSENTIRE L'AUTENTICAZIONE DEL PERSONAL TRAINER

        [HttpPost]
        public ActionResult CercaTrainer()
        {
            //verifica se il trainer è nel database
            if(ModelState.IsValid)
                try
                {
                    string email = Request["Email"];
                    string password = Request["Password"];
                    if (wcfClient.CercaPersonalTrainerNelDB(email, password))
                    {
                        UserDTO tmpTrainer = wcfClient.GetUserByEmail(email);
                        loggedTrainer = new TrainerModel(tmpTrainer.codice_fiscale, tmpTrainer.nome, tmpTrainer.cognome, tmpTrainer.mail, tmpTrainer.data_nascita, tmpTrainer.telefono, tmpTrainer.password, tmpTrainer.sesso);
                        return View("MainPageTrainer", loggedTrainer); //PER ORA LO REINDIRIZZO IN HOME PAGE
                    }
                    else return View("DatiErratiLogin");
                }
                catch(Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("ErrorPage");
                }
            return View("ErrorPage");
        }

        [HttpPost]
        public ActionResult CercaClientiConTrainer()
        {
            ///Questo metodo esegue una query cercando tutti i clienti associati
            ///ad un determinato trainer. Si passa alla view una lista di oggetti
            ///di tipo cliente e se ne stampano i dati.
            List<UserDTO> risultato = new List<UserDTO>();
            List<ClienteModel> clienti = new List<ClienteModel>();
            if(ModelState.IsValid)
            {
                try
                {
                    if ((risultato=wcfClient.CercaClientiAssociatiAlTrainer(loggedTrainer.Codice_fiscale).ToList()) != null)
                    {
                        foreach (UserDTO user in risultato)
                            clienti.Add(new ClienteModel(user.codice_fiscale, user.nome, user.cognome, user.mail, user.data_nascita, user.telefono, user.password, user.sesso));
                        List<ClienteModel> clientiNonDoppioni = new List<ClienteModel>();
                        foreach(ClienteModel cliente in clienti)
                            if (clientiNonDoppioni.Count(c => c.Codice_fiscale == cliente.Codice_fiscale) == 0)
                                clientiNonDoppioni.Add(cliente);
                        return View("RiepilogoClienti", clientiNonDoppioni);
                    }
                    else return View("MainPageTrainer");
                }
                catch(Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("ErrorPage");
                }
            }
            return View("ErrorPage");
        }
    }
}