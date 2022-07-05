using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
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
            loggedClient = null;
            loggedTrainer = null;
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
            return View("ErrorPage");
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
                    dynamic mymodel = new ExpandoObject();
                    if (c != null)
                    {
                        ContrattoModel activeContract = new ContrattoModel(c.Id, c.Descrizione, c.Prezzo, c.Durata);
                        int giorni_scadenza = (c.Data_inizio.AddMonths(activeContract.Durata) - DateTime.Now).Days;
                        mymodel.Cliente = loggedClient;
                        mymodel.Contratto = activeContract;
                        mymodel.GiorniScadenza = giorni_scadenza;
                        mymodel.DataInizio = c.Data_inizio;
                        mymodel.DataScadenza = c.Data_inizio.AddMonths(c.Durata);
                    }
                    else
                    {
                        mymodel.Cliente = loggedClient;
                        mymodel.Contratto = null;
                    }
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

        /// VIENE RICHIAMATO QUANDO L'UTENTE HA UN ABBONAMENTO SCADUTO E NE VUOLE SOTTOSCRIVERE UNO NUOVO
        [HttpPost]
        public ActionResult ReindirizzamentoCreazioneContratto()
        {
            try
            {
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
        

        /// VIENE RICHIAMATO QUANDO L'UTENTE DALLA SUA AREA PERSONALE VUOLE VISUALIZZARE LA PROPRIA ATTUALE
        /// SCHEDA DI ALLENAMENTO
        [HttpPost]
        public ActionResult ShowScheda()
        {
            try
            {
                SchedaDTO schedaAttivaDTO = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList().FirstOrDefault(scheda => scheda.In_uso == true); ///torna la prima scheda che viene trovata come attiva
                if (schedaAttivaDTO == null)
                {
                    ///recupero i dati dell'utimo allenatore che ha creato una scheda all'utente in modo che l'utente possa contattarlo
                    List<SchedaDTO> schede_cliente = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList();
                    DateTime max = schede_cliente[0].Data_inizio;
                    foreach (SchedaDTO scheda in schede_cliente)
                        if (scheda.Data_inizio.CompareTo(max) > 0)
                            max = scheda.Data_inizio;
                    SchedaDTO s = schede_cliente.Find(scheda => scheda.Data_inizio == max);
                    UserDTO selected_trainerDTO = wcfClient.GetUserByEmail(s.Mail_trainer);
                    TrainerModel selected_trainer = new TrainerModel(selected_trainerDTO.codice_fiscale, selected_trainerDTO.nome, selected_trainerDTO.cognome, selected_trainerDTO.mail, selected_trainerDTO.data_nascita, selected_trainerDTO.telefono, selected_trainerDTO.password, selected_trainerDTO.sesso);
                    return View("SchedaMancante",selected_trainer);
                }
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

        /// VIENE RICHIAMATO QUANDO L'UTENTE DALLA SUA AREA PERSONALE VUOLE VISUALIZZARE TUTTE LE SCHEDE DA 
        /// LUI USATE
        [HttpPost]
        public ActionResult ShowAllSchede()
        {
            try
            {
                List<SchedaDTO> listaSchedeAttiveDTO = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList();
                List<SchedaModel> schede = new List<SchedaModel>();
                ///inizializzazione schede
                foreach (SchedaDTO scheda in listaSchedeAttiveDTO)
                {
                    UserDTO trainerDTO = wcfClient.GetUserByEmail(scheda.Mail_trainer);
                    TrainerModel trainer = new TrainerModel(trainerDTO.codice_fiscale, trainerDTO.nome, trainerDTO.cognome, trainerDTO.mail, trainerDTO.data_nascita, trainerDTO.telefono, trainerDTO.password, trainerDTO.sesso);
                    SchedaModel scheda_attuale = new SchedaModel(scheda.Id, scheda.Titolo, scheda.Durata, scheda.In_uso, trainer, loggedClient,new List<EsercizioModel>(), new List<CaratteristicaEsercizioModel>());
                    foreach (EsercizioDTO ese in scheda.Esercizi)
                        scheda_attuale.Esercizi.Add(new EsercizioModel(ese.Descrizione, ese.Immagine));
                    foreach (CaratteristicaEsercizioDTO cara in scheda.Caratteristica_esercizi)
                        scheda_attuale.Caratteristiche_esercizi.Add(new CaratteristicaEsercizioModel(cara.Num_ripetizioni, cara.Recupero, cara.Commento));
                    schede.Add(scheda_attuale);
                }
                return View(schede);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        [HttpPost]
        public ActionResult ContattaAllenatore()
        {
            string mail_trainer = Request["trainerMail"];
            dynamic mymodel = new ExpandoObject();
            mymodel.Mail_trainer = mail_trainer;
            return View(mymodel);
        }

        [HttpPost]
        public ActionResult InviaMessaggioTrainer()
        {
            string mail_trainer = Request["mail_trainer"];
            string messaggio = Request["messaggio"];
            string from = loggedClient.Email;
            MailMessage message = new MailMessage(from, mail_trainer);
            message.Subject = "Richiesta creazione nuova scheda";
            message.Body = messaggio;
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            // Credentials are necessary if the server requires the client
            // to authenticate before it will send email on the client's behalf.
            client.UseDefaultCredentials = true;
            try
            {
                client.Send(message);
                return null;
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

        [HttpPost]
        public ActionResult IniziaCreazioneScheda()
        {
            if(ModelState.IsValid)
            {
                try
                {
                    string email = Request["email"]; //tirati giù dall'input type nascosto
                    string cod_fiscale_trainer = loggedTrainer.Codice_fiscale;
                    UserDTO tmpCliente = wcfClient.GetUserByEmail(email);

                    //mantengo aggiornato il cliente loggato
                    loggedClient = new ClienteModel(tmpCliente.codice_fiscale, tmpCliente.nome, tmpCliente.cognome, tmpCliente.mail, tmpCliente.data_nascita, tmpCliente.telefono, tmpCliente.password, tmpCliente.sesso);
                    string cod_fiscale_cliente = loggedClient.Codice_fiscale;

                    bool result = wcfClient.AggiornaUtilizzoSchede(cod_fiscale_trainer, cod_fiscale_cliente);
                    if(!result)
                    {
                        ViewData["ErrorMessage"] = "Errore nell'aggiornare lo stato di inattività delle schede, la preghiamo di tornare indietro e riprovare!";
                        return View("ErrorPage");
                    }
                    return View("CreazioneScheda", loggedClient);
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("ErrorPage");
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult AggiungiNuovaScheda()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //loggedClient è stato già aggiornato nei passi precedenti
                    string titolo = Request["Titolo"];
                    int durata = Int32.Parse(Request["Durata"]);
                    bool success = wcfClient.InserisciSchedaNelDB(titolo, durata);
                    if(!success)
                    {
                        ViewData["ErrorMessage"] = "Errore nell'istanziazione delle scheda, la preghiamo di tornare indietro e riprovare!";
                        return View("ErrorPage");
                    }
                    //ottenere id dell'ultima scheda inserita nel db

                    int idScheda = wcfClient.OttieniIdUtimaSchedaInserita();
                    if(idScheda==-1)
                    {
                        ViewData["ErrorMessage"] = "Errore nel recupero dell'id della scheda, la preghiamo di tornare indietro e riprovare!";
                        return View("ErrorPage");
                    }

                    success = wcfClient.AggiungiNuovaAssegnazione(loggedTrainer.Codice_fiscale, loggedClient.Codice_fiscale, idScheda, generateStringByDateForMySql(DateTime.Now));
                    if (!success)
                    {
                        ViewData["ErrorMessage"] = "Errore nell'aggiornare l'assegnazione della scheda, la preghiamo di tornare indietro e riprovare!";
                        return View("ErrorPage");
                    }
                    return View("Index"); //sistemare il riferimento alla pagina 
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = ex.Message;
                    return View("ErrorPage");
                }
            }
            return null;
        }
    }
}