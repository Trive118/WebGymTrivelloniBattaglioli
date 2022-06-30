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
    prezzo double(4,2) NOT NULL,
    durata int(2) NOT NULL,
    
    CONSTRAINT ControlloDurata CHECK(durata>0),
    CONSTRAINT ControlloPrezzo CHECK(prezzo>0.00)
)

