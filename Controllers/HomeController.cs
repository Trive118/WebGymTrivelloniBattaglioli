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
        static ClienteModel loggedClient; //cerco sempre di mantenere in memoria chi è il cliente loggato
        static TrainerModel loggedTrainer; //cerco sempre di mantenere in memoria chi è il trainer loggato
        ServiceClient wcfClient = new ServiceReferenceWCF.ServiceClient();
        
        ///Ogni volta che un utente (cliente o trainer) viene reindirizzato in home page viene 
        ///implicitamente chiamata un'istruzione di logout;
        public ActionResult Index()
        {
            loggedClient = null; //logout cliente
            loggedTrainer = null; //logout trainer
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
            if (ModelState.IsValid)  ///controllo se il form è stato compilato correttamente
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
                //si richiama il metodo InserisciCliente del WCF Service
                bool success = wcfClient.InserisciCliente(loggedClient.Codice_fiscale, loggedClient.Nome, loggedClient.Cognome, loggedClient.Email, generateStringByDateForMySql(loggedClient.Data_nascita), loggedClient.Telefono, loggedClient.Password, sesso);
                if(!success)
                {
                    ViewData["ErrorMessage"] = "Errore nell'inserimento dei tuoi dati nel server, la preghiamo di riprovare!";
                    return View("ErrorPage");
                }
                ///DOWLOAD DI TUTTI I CONTRATTI DISPONIBILI DA MOSTRARE ALL'UTENTE NELLA SEZIONE SUCCESSIVA
                List<ContractDTO> contracts_generics = wcfClient.GetAvailableContracts().ToList();
                List<ContrattoModel> contracts = new List<ContrattoModel>();
                foreach (ContractDTO contract in contracts_generics)
                    contracts.Add(new ContrattoModel(contract.Id, contract.Descrizione, contract.Prezzo, contract.Durata));
                return View("ChooseContract", contracts); //passo alla view "ChooseContract" una lista di contratti che verranno mostrati dentro ad una table
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
                string data_iscrizione = generateStringByDateForMySql(DateTime.Now); //chiamo il metodo dichiarato poco più sopra
                //chiamata al metodo AddContractToClient del WCF Service con passaggio parametri
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
                //se viene sollevata un'eccezione viene mostrata un'opportuna pagina di errore con una serie di azioni suggerite
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        /// VIENE RICHIAMATO QUANDO L'UTENTE IMMETTE LE SUE CREDENZIALI NEL FORM DI LOGIN.
        /// INTERROGA IL DATABASE TRAMITE WCF E SE L'UTENTE SI è LOGGATO CORRETTAMENTE GLI PERMETTE
        /// DI ENTRARE NELLA SUA AREA PERSONALE, ALTRIMENTI VISUALIZZA UNA PAGINA DI ERRORE CON UNA SERIE
        /// DI AZIONI CONSIGLIATE.
        [HttpPost]
        public ActionResult EffettuaLogin()
        {
            try
            {
                string mail = Request["Email"];
                string password = Request["Password"];
                if (wcfClient.ConvalidLogIn(mail, password))
                {
                    UserDTO user = wcfClient.GetUserByEmail(mail); //il metodo trova i dati della persona con la mail passata per argomento
                    loggedClient = new ClienteModel(user.codice_fiscale,user.nome,user.cognome,user.mail,user.data_nascita,user.telefono,user.password,user.sesso); //aggiorno i dati del loggedClient
                    ContractDTO c = wcfClient.GetUserActiveContract(loggedClient.Codice_fiscale);
                    dynamic mymodel = new ExpandoObject(); //dichiarazione oggetto dinamico per mantenere in memoria più informazioni (del cliente e del contratto)
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

        /// VIENE RICHIAMATO QUANDO L'UTENTE HA UN ABBONAMENTO SCADUTO (O ASSENTE) E NE VUOLE SOTTOSCRIVERE UNO NUOVO
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
        /// SCHEDA DI ALLENAMENTO.
        [HttpPost]
        public ActionResult ShowScheda()
        {
            try
            {
                if(wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale)== null)
                {
                    ///recupero i dati dell'utimo allenatore che ha creato una scheda all'utente in modo che l'utente possa contattarlo
                    List<UserDTO> all_trainerDTO = wcfClient.OttieniListaTrainer().ToList();
                    List<TrainerModel> all_trainer = new List<TrainerModel>();
                    foreach (UserDTO user in all_trainerDTO)
                        all_trainer.Add(new TrainerModel(user.codice_fiscale,user.nome,user.cognome));
                    return View("SchedaMancante", all_trainer);
                }
                SchedaDTO schedaAttivaDTO = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList().FirstOrDefault(scheda => scheda.In_uso == true); ///torna la prima scheda che viene trovata come attiva
                ///schedaAttivaDTO è null quando il cliente ha una o più schede, ma sono tutte non in uso
                if(schedaAttivaDTO == null)
                {
                    List<UserDTO> all_trainerDTO = wcfClient.OttieniListaTrainer().ToList();
                    List<TrainerModel> all_trainer = new List<TrainerModel>();
                    foreach (UserDTO user in all_trainerDTO)
                        all_trainer.Add(new TrainerModel(user.codice_fiscale, user.nome, user.cognome));
                    return View("SchedaMancante", all_trainer);
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

        /// VIENE RICHIAMATO QUANDO L'UTENTE DALLA SUA AREA PERSONALE VUOLE VISUALIZZARE TUTTE LE SCHEDE A
        /// LUI ASSOCIATE DA QUANDO SI E' ISCRITTO ALLA PALESTRA
        [HttpPost]
        public ActionResult ShowAllSchede()
        {
            try
            {
                List<SchedaDTO> listaSchedeAttiveDTO = new List<SchedaDTO>();
                //Quando il cliente non ha neanche una scheda associata alla sua utenza si rende necessario 
                //un controllo. Il cliente viene pertanto reindirizzato sulla pagina per contattare il trainer
                //e farsi assegnare una scheda di prova.
                if(wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale) == null) //se il cliente non ha liste attive
                {
                    List<UserDTO> lista_trainerDTO = wcfClient.OttieniListaTrainer().ToList(); //tiro giù la lista di trainer
                    List<TrainerModel> lista_trainer = new List<TrainerModel>();
                    foreach (var t in lista_trainerDTO)
                        lista_trainer.Add(new TrainerModel(t.codice_fiscale, t.nome, t.cognome));
                    return View("SchedaMancante",lista_trainer);
                }
                else
                {
                    listaSchedeAttiveDTO = wcfClient.GetSchedeUtente(loggedClient.Codice_fiscale).ToList();
                    List<SchedaModel> schede = new List<SchedaModel>();
                    ///inizializzazione schede
                    foreach (SchedaDTO scheda in listaSchedeAttiveDTO)
                    {
                        UserDTO trainerDTO = wcfClient.GetUserByEmail(scheda.Mail_trainer);
                        TrainerModel trainer = new TrainerModel(trainerDTO.codice_fiscale, trainerDTO.nome, trainerDTO.cognome, trainerDTO.mail, trainerDTO.data_nascita, trainerDTO.telefono, trainerDTO.password, trainerDTO.sesso);
                        SchedaModel scheda_attuale = new SchedaModel(scheda.Id, scheda.Titolo, scheda.Durata, scheda.In_uso, trainer, loggedClient, new List<EsercizioModel>(), new List<CaratteristicaEsercizioModel>());
                        foreach (EsercizioDTO ese in scheda.Esercizi)
                            scheda_attuale.Esercizi.Add(new EsercizioModel(ese.Descrizione, ese.Immagine));
                        foreach (CaratteristicaEsercizioDTO cara in scheda.Caratteristica_esercizi)
                            scheda_attuale.Caratteristiche_esercizi.Add(new CaratteristicaEsercizioModel(cara.Num_ripetizioni, cara.Recupero, cara.Commento));
                        schede.Add(scheda_attuale);
                    }
                    return View(schede);
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        /// Metodo ideato per fare in modo che al cliente, sprovvisto di scheda, ne venga asseganta una
        /// da parte di uno dei trainer della palestra. Il trainer viene selezionato dal cliente
        /// da interfaccia web tramite menù a tendina.
        [HttpPost]
        public ActionResult ContattaAllenatore()
        {
            string cod_trainer = Request["lista_trainer"];
            string nomeCognome = loggedClient.Nome +" "+ loggedClient.Cognome;
            bool result = wcfClient.AssegnaPrimaSchedaAlCliente(loggedClient.Codice_fiscale, cod_trainer,nomeCognome);
            if (result)
                return View();
            else
                return View("ErrorPage");
        }

        /// METODO PER CONSENTIRE L'AUTENTICAZIONE DEL PERSONAL TRAINER
        /// Si verifica la correttezza dei dati inseriti tramite un meteodo contenuto
        /// nel WCFService. In caso di errore viene mostrata una pagina di errore.

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
            ///ad un determinato trainer. Il trainer è quello che si è loggato Si passa alla view una lista di oggetti
            ///di tipo cliente e se ne stampano i dati.
            List<UserDTO> risultato = new List<UserDTO>();
            List<ClienteModel> clienti = new List<ClienteModel>();
            if(ModelState.IsValid)
            {
                try
                {
                    //if ((risultato=wcfClient.CercaClientiAssociatiAlTrainer(loggedTrainer.Codice_fiscale).ToList()) != null)
                    if(wcfClient.CercaClientiAssociatiAlTrainer(loggedTrainer.Codice_fiscale) != null)
                    {
                        risultato = wcfClient.CercaClientiAssociatiAlTrainer(loggedTrainer.Codice_fiscale).ToList();
                        foreach (UserDTO user in risultato)
                            clienti.Add(new ClienteModel(user.codice_fiscale, user.nome, user.cognome, user.mail, user.data_nascita, user.telefono, user.password, user.sesso));
                        List<ClienteModel> clientiNonDoppioni = new List<ClienteModel>();
                        foreach(ClienteModel cliente in clienti)
                            if (clientiNonDoppioni.Count(c => c.Codice_fiscale == cliente.Codice_fiscale) == 0)
                                clientiNonDoppioni.Add(cliente);
                                
                        //Questa lista di bool viaggia in parallelo alla lista di clientiNonDoppioni
                        List<bool> allenamentoScaduto = new List<bool>();
                        foreach (ClienteModel cliente in clientiNonDoppioni)
                        {
                            if (wcfClient.ControlloSchedaScaduta(cliente.Codice_fiscale)) //il cliente non ha una scheda oppure la più recente tra tutte gli è scaduta
                                allenamentoScaduto.Add(true);
                            else
                                allenamentoScaduto.Add(false); //il cliente ha una scheda ancora attiva
                        }
                        dynamic mymodel = new ExpandoObject();
                        mymodel.Clienti = clientiNonDoppioni;
                        mymodel.Scaduti = allenamentoScaduto;
                        return View("RiepilogoClienti", mymodel);
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

        ///Questo metodo rappresenta l'inizio dell'iter di creazione di una scheda per il cliente.
        [HttpPost]
        public ActionResult IniziaCreazioneScheda()
        {
            if(ModelState.IsValid)
            {
                string email = Request["email"]; //tirati giù dall'input type nascosto
                try
                {
                    string cod_fiscale_trainer = loggedTrainer.Codice_fiscale;
                    UserDTO tmpCliente = wcfClient.GetUserByEmail(email); //vado a cercare il cliente con quell'indirizzo email nel database
                    ///Nota bene: sul DB c'è un vincolo di UNIQUE nel campo email per cui non possono esistere
                    ///due clienti con la stessa email.

                    //mantengo aggiornato il cliente loggato
                    loggedClient = new ClienteModel(tmpCliente.codice_fiscale, tmpCliente.nome, tmpCliente.cognome, tmpCliente.mail, tmpCliente.data_nascita, tmpCliente.telefono, tmpCliente.password, tmpCliente.sesso);
                    string cod_fiscale_cliente = loggedClient.Codice_fiscale;

                    //Imposto come "non in uso" tutte le schede associate al cliente
                    //dal momento in cui si è iscritto nella palestra.
                    bool result = wcfClient.AggiornaUtilizzoSchede(tmpCliente.codice_fiscale);
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

        ///Questo metodo raccoglie i dati dal form contenuto nella pagina web. Si crea quindi una nuova
        ///riga nella tabella scheda con i dati relativi a titolo e durata. L'id, essendo settato come
        ///AUTO_INCREMENT, viene gestito direttamente dal database.
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
                    string cod_cliente = Request["cod_utente"];
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
                    success = wcfClient.AggiungiNuovaAssegnazione(loggedTrainer.Codice_fiscale, loggedClient.Codice_fiscale, idScheda, DateTime.Now.ToString("yyyy-MM-dd"));
                    if (!success)
                    {
                        ViewData["ErrorMessage"] = "Errore nell'aggiornare l'assegnazione della scheda, la preghiamo di tornare indietro e riprovare!";
                        return View("ErrorPage");
                    }
                    List<EsercizioModel> TuttiEsercizi = new List<EsercizioModel>();
                    List<EsercizioDTO> TuttiEserciziDTO = new List<EsercizioDTO>();
                    TuttiEserciziDTO = wcfClient.GetAllExercise().ToList();
                    foreach (EsercizioDTO es in TuttiEserciziDTO)
                        TuttiEsercizi.Add(new EsercizioModel(es.Descrizione, es.Immagine));
                    dynamic mymodel = new ExpandoObject();
                    mymodel.Id_scheda = idScheda;
                    mymodel.Esercizi = null;
                    mymodel.EserciziDisponibili = TuttiEsercizi;
                    return View("CreazioneSchedaEsercizi",mymodel); 
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
        public ActionResult LogOutCliente()
        {
            try
            {
                loggedClient = null;
                //setto il loggedClient usando i parametri (vuoti) del clienteVuoto;
                return View("LogIn", loggedClient);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        [HttpPost]
        public ActionResult LogOutTrainer()
        {
            try
            {
                loggedTrainer = null;
                return View("Contact", loggedTrainer);
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }


        ///Metodo per aggiungere il riferimento tra scheda (istanziata già a suo tempo) e un esercizio.
        [HttpPost]
        public ActionResult AddEsercizioScheda()
        {
            //Prendo i dati dal form
            string descrizione = Request["Descrizione"];
            int num_ripetizioni = Convert.ToInt32(Request["Num_ripetizioni"]);
            int tempo_recupero_app = Convert.ToInt32(Request["Recupero"]);
            TimeSpan tempo_recupero = TimeSpan.FromSeconds(tempo_recupero_app);
            string commento = Request["Commento"];
            string immagine = Request["Immagine"];
            string select_value = Request["esercizio_select"];
            int id_scheda = Convert.ToInt32(Request["id_scheda"]);
            try
            {
                bool result;

                if(select_value != "NULL") //il trainer ha scelto un esercizio che esiste già nel DB
                    result = wcfClient.AddExerciseToCardGym(descrizione, id_scheda, num_ripetizioni, tempo_recupero, commento);
                else //il trainer sta creando un nuovo esercizio 
                    result = wcfClient.AddNewExerciseToCardGym(id_scheda, descrizione, num_ripetizioni, tempo_recupero, commento, immagine);
                
                List<EsercizioModel> lista_esercizi = new List<EsercizioModel>();
                List<EsercizioModel> TuttiEsercizi = new List<EsercizioModel>();
                List<EsercizioDTO> TuttiEserciziDTO = new List<EsercizioDTO>();
                List<CaratteristicaEsercizioModel> lista_caratteristicheEsercizi = new List<CaratteristicaEsercizioModel>();
                EsercizioDTO[] eserciziDTO = Array.Empty<EsercizioDTO>();
                CaratteristicaEsercizioDTO[] caratteristicheDTO = Array.Empty<CaratteristicaEsercizioDTO>();

                //si prendono tutti gli esercizi associati alla scheda del cliente con relative caratteristiche.
                //Da intefaccia web si vedono, in tempo reale, tutti gli esercizi associati a quella determinata scheda
                //che il trainer stra creando il quel momento.

                wcfClient.OttieniEserciziAssociatiAllaScheda(id_scheda, ref eserciziDTO, ref caratteristicheDTO);
                int i = 0;

                //i dati vengono passati al model
                foreach(EsercizioDTO es in eserciziDTO.ToList())
                {
                    lista_esercizi.Add(new EsercizioModel(es.Descrizione, es.Immagine));
                    lista_caratteristicheEsercizi.Add(new CaratteristicaEsercizioModel(caratteristicheDTO[i].Num_ripetizioni, caratteristicheDTO[i].Recupero, caratteristicheDTO[i].Commento));
                    i++;
                }

                //Lista di tutti gli esercizi disponibili che andiamo a recuperare dal DB in modo che vegano mostrati
                //nell'interfaccia web in un'apposita dropdown-list. Il trainer vede quindi un menù a tendina dal quale
                //seleziona un apposito esercizio.
                TuttiEserciziDTO = wcfClient.GetAllExercise().ToList();
                foreach (EsercizioDTO es in TuttiEserciziDTO)
                    TuttiEsercizi.Add(new EsercizioModel(es.Descrizione, es.Immagine));
                
                
                dynamic mymodel = new ExpandoObject(); //oggetto dinamico con id_scheda, lista di esercizi associati alla scheda, lista di caratteristiche associate agli esercizi e tutti gli esercizi presenti sul DB
                //Questo oggetto dinamico viene passato alla View in modo che da interfaccia web si possano richiamare tutti i dati che servono.
                mymodel.Id_scheda = id_scheda;
                mymodel.Esercizi = lista_esercizi;
                mymodel.CaratteristicheEsercizi = lista_caratteristicheEsercizi;
                mymodel.EserciziDisponibili = TuttiEsercizi;
                return View("CreazioneSchedaEsercizi", mymodel);
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }
        }

        [HttpPost]
        public ActionResult BackToTheHomeClient()
        {
            try
            {
                ContractDTO c = wcfClient.GetUserActiveContract(loggedClient.Codice_fiscale);
                dynamic mymodel = new ExpandoObject(); //dichiarazione oggetto dinamico per mantenere in memoria più informazioni (del cliente e del contratto)
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
            }catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ErrorPage");
            }

            
        }

        [HttpPost]
        public ActionResult ConfirmScheda()
        {
            return View(loggedClient);
        }

        public ActionResult TornaAllaHome()
        {
            return View("MainPageTrainer",loggedTrainer);
        }

    }
}