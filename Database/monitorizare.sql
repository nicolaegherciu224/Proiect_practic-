-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 18, 2026 at 10:29 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `monitorizare`
--

-- --------------------------------------------------------

--
-- Table structure for table `conturi`
--

CREATE TABLE `conturi` (
  `ID_account` int(11) NOT NULL,
  `login` varchar(50) DEFAULT NULL,
  `parola` varchar(255) DEFAULT NULL,
  `tip_drepturi` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `conturi`
--

INSERT INTO `conturi` (`ID_account`, `login`, `parola`, `tip_drepturi`) VALUES
(1, 'superadmin', '4a7372bd055b0d93fbd42915bd03a7d6a58c81f9e0be5ac9fe8f57c2b5c359b5', 'Super Admin'),
(2, 'admin', 'aa163422e098f8a463b38e5dedbe08d133d48fac0a8693582a48d3db59cd2a06', 'Admin'),
(3, 'inginer_vlad', '98f50f0bc982c8bd2ff8c0a6e223cd4a9c033806a8084f8b22732c2b60bc49af', 'Inginer');

-- --------------------------------------------------------

--
-- Table structure for table `echipamente`
--

CREATE TABLE `echipamente` (
  `ID_echipament` int(11) NOT NULL,
  `tip_echipament` varchar(50) DEFAULT NULL,
  `model` varchar(50) DEFAULT NULL,
  `nr_serie` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `echipamente`
--

INSERT INTO `echipamente` (`ID_echipament`, `tip_echipament`, `model`, `nr_serie`) VALUES
(1, 'Cantar Electronic', 'CAS CL5200', 'CAS20260192A'),
(2, 'Cantar Electronic', 'T-Scale T28', 'TS20269911B'),
(3, 'Imprimanta Etichete', 'Bixolon SRP-E300', 'BIX77110293'),
(4, 'Imprimanta Termica', 'Bixolon SRP-350', 'BIX35099112'),
(5, 'Scanner Coduri', 'Datalogic QuickScan', 'DLQ55441122'),
(6, 'Scanner Coduri', 'Honeywell Voyager', 'HWV88334411'),
(7, 'Casa de Marcat', 'Datecs DP-25', 'DT25MD00192'),
(8, 'Casa de Marcat', 'Tremol M20', 'TRM20MD0055'),
(9, 'Terminal POS', 'Sunmi T2', 'SNM99114422'),
(10, 'Sistem POS', 'Syrve Box v2', 'SRV20268811');

-- --------------------------------------------------------

--
-- Table structure for table `ingineri`
--

CREATE TABLE `ingineri` (
  `ID_inginer` int(11) NOT NULL,
  `nume` varchar(50) DEFAULT NULL,
  `prenume` varchar(50) DEFAULT NULL,
  `cantitate_echipamente` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `ingineri`
--

INSERT INTO `ingineri` (`ID_inginer`, `nume`, `prenume`, `cantitate_echipamente`) VALUES
(1, 'Cojocaru', 'Vladislav', 3),
(2, 'Munteanu', 'Alexandru', 2),
(3, 'Ceban', 'Ion', 4),
(4, 'Rotari', 'Dumitru', 3),
(5, 'Rusu', 'Andrei', 3),
(6, 'Grosu', 'Mihai', 2),
(7, 'Balan', 'Sergiu', 2),
(8, 'Morari', 'Nicolae', 3),
(9, 'Radu', 'Gheorghe', 2),
(10, 'Sandu', 'Vitalie', 2),
(11, 'Popa', 'Adrian', 2),
(12, 'Lupu', 'Cristian', 2),
(13, 'Cazacu', 'Maxim', 1),
(14, 'Ursu', 'Denis', 1),
(15, 'Vieru', 'Artiom', 0),
(16, 'Sirbu', 'Eugen', 0),
(17, 'Albu', 'Marin', 0),
(18, 'Tabara', 'Oleg', 0),
(19, 'Calmîș', 'Victor', 0),
(20, 'Borta', 'Igor', 0),
(21, 'Melnic', 'Pavel', 0),
(22, 'Gutu', 'Valeriu', 0);

-- --------------------------------------------------------

--
-- Table structure for table `solicitari`
--

CREATE TABLE `solicitari` (
  `nr_solicitare` int(11) NOT NULL,
  `nume_inginer` varchar(50) DEFAULT NULL,
  `prenume_inginer` varchar(50) DEFAULT NULL,
  `client` varchar(100) DEFAULT NULL,
  `adresa` varchar(150) DEFAULT NULL,
  `data` date DEFAULT CURRENT_DATE,
  `tip_echipament` varchar(50) DEFAULT NULL,
  `model_echipament` varchar(50) DEFAULT NULL,
  `cantitate` int(11) DEFAULT NULL,
  `suma` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `solicitari`
--

INSERT INTO `solicitari` (`nr_solicitare`, `nume_inginer`, `prenume_inginer`, `client`, `adresa`, `data`, `tip_echipament`, `model_echipament`, `cantitate`, `suma`) VALUES
(1, 'Cojocaru', 'Vladislav', 'Linella 40', 'str. Moscova 11, Chisinau', '2026-05-02', 'Cantar Electronic', 'CAS CL5200', 2, 800.00),
(2, 'Cojocaru', 'Vladislav', 'Nr1 Supermarket', 'str. Lev Tolstoi 24, Chisinau', '2026-05-04', 'Scanner Coduri', 'Datalogic QuickScan', 1, 400.00),
(3, 'Munteanu', 'Alexandru', 'Rogob SRL', 'bd. Mircea cel Batran 5, Chisinau', '2026-05-05', 'Imprimanta Etichete', 'Bixolon SRP-E300', 1, 400.00),
(4, 'Ceban', 'Ion', 'Fidesco Ciocana', 'str. Alecu Russo 20, Chisinau', '2026-05-06', 'Cantar Electronic', 'T-Scale T28', 2, 800.00),
(5, 'Ceban', 'Ion', 'Local Discounter', 'str. Ceucari 2, Chisinau', '2026-05-08', 'Casa de Marcat', 'Datecs DP-25', 2, 800.00),
(6, 'Rotari', 'Dumitru', 'Dulcinella', 'str. Alba Iulia 194, Chisinau', '2026-05-10', 'Terminal POS', 'Sunmi T2', 1, 400.00),
(7, 'Rotari', 'Dumitru', 'La Placinte', 'bd. Dacia 31, Chisinau', '2026-05-12', 'Sistem POS', 'Syrve Box v2', 2, 800.00),
(8, 'Rusu', 'Andrei', 'Andys Pizza', 'str. Puskin 32, Chisinau', '2026-05-13', 'Imprimanta Termica', 'Bixolon SRP-350', 2, 800.00),
(9, 'Rusu', 'Andrei', 'Pegas Retail', 'str. Albisoara 42, Chisinau', '2026-05-15', 'Scanner Coduri', 'Honeywell Voyager', 1, 400.00),
(10, 'Grosu', 'Mihai', 'Kaufland Botanica', 'str. Decebal 99, Chisinau', '2026-05-16', 'Cantar Electronic', 'CAS CL5200', 2, 800.00),
(11, 'Balan', 'Sergiu', 'Metro Cash&Carry', 'str. Chisinau 5, Stauceni', '2026-05-18', 'Scanner Coduri', 'Datalogic QuickScan', 2, 800.00),
(12, 'Morari', 'Nicolae', 'Petrom Moldova', 'str. Calea Orheiului 100, Chisinau', '2026-05-20', 'Casa de Marcat', 'Tremol M20', 2, 800.00),
(13, 'Morari', 'Nicolae', 'VeloMarket', 'str. Columna 144, Chisinau', '2026-05-21', 'Scanner Coduri', 'Honeywell Voyager', 1, 400.00),
(14, 'Radu', 'Gheorghe', 'Farmacia Felicia', 'bd. Stefan cel Mare 64, Chisinau', '2026-05-22', 'Imprimanta Termica', 'Bixolon SRP-350', 2, 800.00),
(15, 'Sandu', 'Vitalie', 'Hippocrates', 'str. Ion Creanga 48, Chisinau', '2026-05-23', 'Casa de Marcat', 'Datecs DP-25', 2, 800.00),
(16, 'Popa', 'Adrian', 'Moldtelecom S.A.', 'bd. Stefan cel Mare 10, Chisinau', '2026-05-25', 'Terminal POS', 'Sunmi T2', 2, 800.00),
(17, 'Lupu', 'Cristian', 'Orange Moldova', 'str. Alba Iulia 75, Chisinau', '2026-05-26', 'Scanner Coduri', 'Datalogic QuickScan', 2, 800.00),
(18, 'Cazacu', 'Maxim', 'Statiunea Petrom 05', 'str. Hancesti 240, Chisinau', '2026-05-27', 'Casa de Marcat', 'Tremol M20', 1, 400.00),
(19, 'Ursu', 'Denis', 'Clinica Sante', 'str. Ismail 84, Chisinau', '2026-05-28', 'Imprimanta Termica', 'Bixolon SRP-350', 1, 400.00),
(20, 'Munteanu', 'Alexandru', 'Librarius Centro', 'str. Mitropolit Varlaam 65, Chisinau', '2026-05-29', 'Scanner Coduri', 'Honeywell Voyager', 1, 400.00),
(21, 'Cojocaru', 'Vladislav', 'Linella 12', 'str. Nicolae Costin 65, Chisinau', '2026-06-01', 'Cantar Electronic', 'CAS CL5200', 7, 2800.00),
(22, 'Munteanu', 'Alexandru', 'Nestro Petrol', 'str. Petricani 21, Chisinau', '2026-06-02', 'Casa de Marcat', 'Tremol M20', 9, 3600.00),
(23, 'Ceban', 'Ion', 'Don Cezar Pizza', 'str. Trandafirilor 13, Chisinau', '2026-06-03', 'Imprimanta Termica', 'Bixolon SRP-350', 5, 2000.00),
(24, 'Rotari', 'Dumitru', 'Salat Caraman', 'str. bd. Decebal 23, Chisinau', '2026-06-04', 'Sistem POS', 'Syrve Box v2', 2, 800.00),
(25, 'Rusu', 'Andrei', 'Global Store', 'str. Calea Iesilor 91, Chisinau', '2026-06-05', 'Scanner Coduri', 'Datalogic QuickScan', 8, 3200.00),
(26, 'Grosu', 'Mihai', 'Darwin Store', 'str. Alecu Russo 1, Chisinau', '2026-06-06', 'Terminal POS', 'Sunmi T2', 10, 4000.00),
(27, 'Balan', 'Sergiu', 'Enter Center', 'bd. Stefan cel Mare 134, Chisinau', '2026-06-08', 'Sistem POS', 'Syrve Box v2', 10, 4000.00),
(28, 'Morari', 'Nicolae', 'Kaufland Codru', 'str. Natalia Gheorghiu 30, Codru', '2026-06-09', 'Cantar Electronic', 'CAS CL5200', 3, 1200.00),
(29, 'Radu', 'Gheorghe', 'Gastrobar', 'str. Alexandru cel Bun 51, Chisinau', '2026-06-10', 'Imprimanta Termica', 'Bixolon SRP-350', 4, 1600.00),
(30, 'Sandu', 'Vitalie', 'Bucuria Magazin', 'str. Columna 162, Chisinau', '2026-06-12', 'Casa de Marcat', 'Datecs DP-25', 7, 2800.00),
(31, 'Popa', 'Adrian', 'Tucano Coffee', 'str. Alexander Puskin 15, Chisinau', '2026-06-14', 'Imprimanta Termica', 'Bixolon SRP-350', 12, 4800.00),
(32, 'Lupu', 'Cristian', 'Franzeluta Magazin', 'str. Botanica Veche 10, Chisinau', '2026-06-15', 'Casa de Marcat', 'Datecs DP-25', 6, 2400.00);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `conturi`
--
ALTER TABLE `conturi`
  ADD PRIMARY KEY (`ID_account`);

--
-- Indexes for table `echipamente`
--
ALTER TABLE `echipamente`
  ADD PRIMARY KEY (`ID_echipament`);

--
-- Indexes for table `ingineri`
--
ALTER TABLE `ingineri`
  ADD PRIMARY KEY (`ID_inginer`);

--
-- Indexes for table `solicitari`
--
ALTER TABLE `solicitari`
  ADD PRIMARY KEY (`nr_solicitare`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `conturi`
--
ALTER TABLE `conturi`
  MODIFY `ID_account` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `echipamente`
--
ALTER TABLE `echipamente`
  MODIFY `ID_echipament` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `ingineri`
--
ALTER TABLE `ingineri`
  MODIFY `ID_inginer` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT for table `solicitari`
--
ALTER TABLE `solicitari`
  MODIFY `nr_solicitare` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
