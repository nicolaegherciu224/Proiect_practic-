-- Schema SQLite pentru aplicația Engineering Management
-- Baza rulează ca fișier local: Database/monitorizare.db
-- Datele inițiale sunt importate automat din monitorizare.sql la prima pornire

CREATE TABLE IF NOT EXISTS conturi (
    ID_account    INTEGER PRIMARY KEY AUTOINCREMENT,
    login         TEXT,
    parola        TEXT,
    tip_drepturi  TEXT
);

CREATE TABLE IF NOT EXISTS echipamente (
    ID_echipament  INTEGER PRIMARY KEY AUTOINCREMENT,
    tip_echipament TEXT,
    model          TEXT,
    nr_serie       TEXT UNIQUE
);

CREATE TABLE IF NOT EXISTS ingineri (
    ID_inginer            INTEGER PRIMARY KEY AUTOINCREMENT,
    nume                  TEXT,
    prenume               TEXT,
    cantitate_echipamente INTEGER
);

CREATE TABLE IF NOT EXISTS solicitari (
    nr_solicitare    INTEGER PRIMARY KEY AUTOINCREMENT,
    nume_inginer     TEXT,
    prenume_inginer  TEXT,
    client           TEXT,
    adresa           TEXT,
    data             TEXT,
    tip_echipament   TEXT,
    model_echipament TEXT,
    nr_serie         TEXT,
    cantitate        INTEGER,
    suma             REAL,
    status           TEXT NOT NULL DEFAULT 'Preluat'
);

CREATE TABLE IF NOT EXISTS solicitari_audit (
    id              INTEGER PRIMARY KEY AUTOINCREMENT,
    nr_solicitare   INTEGER NOT NULL,
    status_vechi    TEXT,
    status_nou      TEXT NOT NULL,
    data_schimbare  TEXT NOT NULL,
    utilizator      TEXT NOT NULL,
    observatii      TEXT
);
