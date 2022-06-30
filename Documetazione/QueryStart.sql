CREATE TABLE Persona
(
    id int(6) PRIMARY KEY AUTO_INCREMENT,
    nome varchar(40) NOT NULL,
    cognome varchar(50) NOT NULL,
    email varchar(70) NOT NULL,
    data_nascita date NOT NULL,
    telefono char(10),
    sesso ENUM('M', 'F') NOT NULL,
    ruolo ENUM('cliente', 'trainer') NOT NULL
    
)

CREATE TABLE Contratto
(
    id int(8) PRIMARY KEY AUTO_INCREMENT,
    descrizione varchar(100) NOT NULL,
    prezzo double(6,2) NOT NULL,
    durata int(2) NOT NULL,
    
    CONSTRAINT ControlloDurata CHECK(durata>0),
    CONSTRAINT ControlloPrezzo CHECK(prezzo>0.00)
)

CREATE TABLE Iscrizione
(
    id int(8) PRIMARY KEY AUTO_INCREMENT,
    data_inizio timestamp NOT NULL,
    motivo varchar(100) NOT NULL,
    idPersona int(6),
    idContratto int(8),
    FOREIGN KEY (idPersona) REFERENCES Persona(id),
    FOREIGN KEY (idContratto) REFERENCES Contratto(id)
    
)

CREATE TABLE Scheda
(
    id int(6) PRIMARY KEY AUTO_INCREMENT,
    idTrainer int(6),
    idCliente int(6),
    titolo varchar(50) NOT NULL,
    durata int(2) NOT NULL,
    
    CONSTRAINT ControlloDurataScheda CHECK(durata>0),
    FOREIGN KEY (idTrainer) REFERENCES Persona(id),
    FOREIGN KEY (idCliente) REFERENCES Persona(id)
)



CREATE TABLE Esercizio
(
    id int(5) PRIMARY KEY AUTO_INCREMENT,
    immagine varchar(100) NOT NULL,
    descrizione varchar(100) NOT NULL
    
)

CREATE TABLE Associazione
(
    idScheda int(6),
    idEsercizio int(5),
    
    numeroRipetizioni int(2) NOT NULL,
    recupero time NOT NULL,
    commento varchar(50),
    
    FOREIGN KEY (idScheda) REFERENCES Scheda(id),
    FOREIGN KEY (idEsercizio) REFERENCES Esercizio(id),
    PRIMARY KEY(idScheda, idEsercizio)
)





