using EngineeringManagementApp.Enums;
using EngineeringManagementApp.Models;

namespace EngineeringManagementApp.Data;

/// <summary>
/// Date inițiale importate din monitorizare.sql (dump MySQL/MariaDB).
/// Se încarcă automat la prima pornire dacă baza SQLite e goală.
/// </summary>
public static class MonitorizareSeedData
{
    public static IReadOnlyList<Cont> Conturi =>
    [
        new() { IdAccount = 1, Login = "superadmin", Parola = "3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121", TipDrepturi = "Super Admin" },
        new() { IdAccount = 2, Login = "admin", Parola = "3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121", TipDrepturi = "Admin" },
        new() { IdAccount = 3, Login = "inginer_vlad", Parola = "3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121", TipDrepturi = "Inginer" }
    ];

    public static IReadOnlyList<Echipament> Echipamente =>
    [
        new() { IdEchipament = 1, TipEchipament = "Cantar Electronic", ModelEchipament = "CAS CL5200", NrSerie = "CAS20260192A" },
        new() { IdEchipament = 2, TipEchipament = "Cantar Electronic", ModelEchipament = "T-Scale T28", NrSerie = "TS20269911B" },
        new() { IdEchipament = 3, TipEchipament = "Imprimanta Etichete", ModelEchipament = "Bixolon SRP-E300", NrSerie = "BIX77110293" },
        new() { IdEchipament = 4, TipEchipament = "Imprimanta Termica", ModelEchipament = "Bixolon SRP-350", NrSerie = "BIX35099112" },
        new() { IdEchipament = 5, TipEchipament = "Scanner Coduri", ModelEchipament = "Datalogic QuickScan", NrSerie = "DLQ55441122" },
        new() { IdEchipament = 6, TipEchipament = "Scanner Coduri", ModelEchipament = "Honeywell Voyager", NrSerie = "HWV88334411" },
        new() { IdEchipament = 7, TipEchipament = "Casa de Marcat", ModelEchipament = "Datecs DP-25", NrSerie = "DT25MD00192" },
        new() { IdEchipament = 8, TipEchipament = "Casa de Marcat", ModelEchipament = "Tremol M20", NrSerie = "TRM20MD0055" },
        new() { IdEchipament = 9, TipEchipament = "Terminal POS", ModelEchipament = "Sunmi T2", NrSerie = "SNM99114422" },
        new() { IdEchipament = 10, TipEchipament = "Sistem POS", ModelEchipament = "Syrve Box v2", NrSerie = "SRV20268811" }
    ];

    public static IReadOnlyList<Inginer> Ingineri =>
    [
        new() { IdInginer = 1, NumeInginer = "Cojocaru", PrenumeInginer = "Vladislav", CantitateEchipamenteDeservite = 3 },
        new() { IdInginer = 2, NumeInginer = "Munteanu", PrenumeInginer = "Alexandru", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 3, NumeInginer = "Ceban", PrenumeInginer = "Ion", CantitateEchipamenteDeservite = 4 },
        new() { IdInginer = 4, NumeInginer = "Rotari", PrenumeInginer = "Dumitru", CantitateEchipamenteDeservite = 3 },
        new() { IdInginer = 5, NumeInginer = "Rusu", PrenumeInginer = "Andrei", CantitateEchipamenteDeservite = 3 },
        new() { IdInginer = 6, NumeInginer = "Grosu", PrenumeInginer = "Mihai", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 7, NumeInginer = "Balan", PrenumeInginer = "Sergiu", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 8, NumeInginer = "Morari", PrenumeInginer = "Nicolae", CantitateEchipamenteDeservite = 3 },
        new() { IdInginer = 9, NumeInginer = "Radu", PrenumeInginer = "Gheorghe", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 10, NumeInginer = "Sandu", PrenumeInginer = "Vitalie", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 11, NumeInginer = "Popa", PrenumeInginer = "Adrian", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 12, NumeInginer = "Lupu", PrenumeInginer = "Cristian", CantitateEchipamenteDeservite = 2 },
        new() { IdInginer = 13, NumeInginer = "Cazacu", PrenumeInginer = "Maxim", CantitateEchipamenteDeservite = 1 },
        new() { IdInginer = 14, NumeInginer = "Ursu", PrenumeInginer = "Denis", CantitateEchipamenteDeservite = 1 },
        new() { IdInginer = 15, NumeInginer = "Vieru", PrenumeInginer = "Artiom", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 16, NumeInginer = "Sirbu", PrenumeInginer = "Eugen", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 17, NumeInginer = "Albu", PrenumeInginer = "Marin", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 18, NumeInginer = "Tabara", PrenumeInginer = "Oleg", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 19, NumeInginer = "Calmîș", PrenumeInginer = "Victor", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 20, NumeInginer = "Borta", PrenumeInginer = "Igor", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 21, NumeInginer = "Melnic", PrenumeInginer = "Pavel", CantitateEchipamenteDeservite = 0 },
        new() { IdInginer = 22, NumeInginer = "Gutu", PrenumeInginer = "Valeriu", CantitateEchipamenteDeservite = 0 }
    ];

    public static IReadOnlyList<Solicitare> Solicitari =>
    [
        S(1, "Cojocaru", "Vladislav", "Linella 40", "str. Moscova 11, Chisinau", "2026-05-02", "Cantar Electronic", "CAS CL5200", 2, 800),
        S(2, "Cojocaru", "Vladislav", "Nr1 Supermarket", "str. Lev Tolstoi 24, Chisinau", "2026-05-04", "Scanner Coduri", "Datalogic QuickScan", 1, 400),
        S(3, "Munteanu", "Alexandru", "Rogob SRL", "bd. Mircea cel Batran 5, Chisinau", "2026-05-05", "Imprimanta Etichete", "Bixolon SRP-E300", 1, 400),
        S(4, "Ceban", "Ion", "Fidesco Ciocana", "str. Alecu Russo 20, Chisinau", "2026-05-06", "Cantar Electronic", "T-Scale T28", 2, 800),
        S(5, "Ceban", "Ion", "Local Discounter", "str. Ceucari 2, Chisinau", "2026-05-08", "Casa de Marcat", "Datecs DP-25", 2, 800),
        S(6, "Rotari", "Dumitru", "Dulcinella", "str. Alba Iulia 194, Chisinau", "2026-05-10", "Terminal POS", "Sunmi T2", 1, 400),
        S(7, "Rotari", "Dumitru", "La Placinte", "bd. Dacia 31, Chisinau", "2026-05-12", "Sistem POS", "Syrve Box v2", 2, 800),
        S(8, "Rusu", "Andrei", "Andy's Pizza", "str. Puskin 32, Chisinau", "2026-05-13", "Imprimanta Termica", "Bixolon SRP-350", 2, 800),
        S(9, "Rusu", "Andrei", "Pegas Retail", "str. Albisoara 42, Chisinau", "2026-05-15", "Scanner Coduri", "Honeywell Voyager", 1, 400),
        S(10, "Grosu", "Mihai", "Kaufland Botanica", "str. Decebal 99, Chisinau", "2026-05-16", "Cantar Electronic", "CAS CL5200", 2, 800),
        S(11, "Balan", "Sergiu", "Metro Cash&Carry", "str. Chisinau 5, Stauceni", "2026-05-18", "Scanner Coduri", "Datalogic QuickScan", 2, 800),
        S(12, "Morari", "Nicolae", "Petrom Moldova", "str. Calea Orheiului 100, Chisinau", "2026-05-20", "Casa de Marcat", "Tremol M20", 2, 800),
        S(13, "Morari", "Nicolae", "VeloMarket", "str. Columna 144, Chisinau", "2026-05-21", "Scanner Coduri", "Honeywell Voyager", 1, 400),
        S(14, "Radu", "Gheorghe", "Farmacia Felicia", "bd. Stefan cel Mare 64, Chisinau", "2026-05-22", "Imprimanta Termica", "Bixolon SRP-350", 2, 800),
        S(15, "Sandu", "Vitalie", "Hippocrates", "str. Ion Creanga 48, Chisinau", "2026-05-23", "Casa de Marcat", "Datecs DP-25", 2, 800),
        S(16, "Popa", "Adrian", "Moldtelecom S.A.", "bd. Stefan cel Mare 10, Chisinau", "2026-05-25", "Terminal POS", "Sunmi T2", 2, 800),
        S(17, "Lupu", "Cristian", "Orange Moldova", "str. Alba Iulia 75, Chisinau", "2026-05-26", "Scanner Coduri", "Datalogic QuickScan", 2, 800),
        S(18, "Cazacu", "Maxim", "Statiunea Petrom 05", "str. Hancesti 240, Chisinau", "2026-05-27", "Casa de Marcat", "Tremol M20", 1, 400),
        S(19, "Ursu", "Denis", "Clinica Sante", "str. Ismail 84, Chisinau", "2026-05-28", "Imprimanta Termica", "Bixolon SRP-350", 1, 400),
        S(20, "Munteanu", "Alexandru", "Librarius Centro", "str. Mitropolit Varlaam 65, Chisinau", "2026-05-29", "Scanner Coduri", "Honeywell Voyager", 1, 400),
        S(21, "Cojocaru", "Vladislav", "Linella 12", "str. Nicolae Costin 65, Chisinau", "2026-06-01", "Cantar Electronic", "CAS CL5200", 4, 1600),
        S(22, "Munteanu", "Alexandru", "Nestro Petrol", "str. Petricani 21, Chisinau", "2026-06-02", "Casa de Marcat", "Tremol M20", 3, 1200),
        S(23, "Ceban", "Ion", "Don Cezar Pizza", "str. Trandafirilor 13, Chisinau", "2026-06-03", "Imprimanta Termica", "Bixolon SRP-350", 1, 400),
        S(24, "Rotari", "Dumitru", "Salat Caraman", "str. bd. Decebal 23, Chisinau", "2026-06-04", "Sistem POS", "Syrve Box v2", 7, 2800),
        S(25, "Rusu", "Andrei", "Global Store", "str. Calea Iesilor 91, Chisinau", "2026-06-05", "Scanner Coduri", "Datalogic QuickScan", 9, 3600),
        S(26, "Grosu", "Mihai", "Darwin Store", "str. Alecu Russo 1, Chisinau", "2026-06-06", "Terminal POS", "Sunmi T2", 10, 4000),
        S(27, "Balan", "Sergiu", "Enter Center", "bd. Stefan cel Mare 134, Chisinau", "2026-06-08", "Sistem POS", "Syrve Box v2", 12, 4800),
        S(28, "Morari", "Nicolae", "Kaufland Codru", "str. Natalia Gheorghiu 30, Codru", "2026-06-09", "Cantar Electronic", "CAS CL5200", 2, 800),
        S(29, "Radu", "Gheorghe", "Gastrobar", "str. Alexandru cel Bun 51, Chisinau", "2026-06-10", "Imprimanta Termica", "Bixolon SRP-350", 6, 2400),
        S(30, "Sandu", "Vitalie", "Bucuria Magazin", "str. Columna 162, Chisinau", "2026-06-12", "Casa de Marcat", "Datecs DP-25", 7, 2800),
        S(31, "Popa", "Adrian", "Tucano Coffee", "str. Alexander Puskin 15, Chisinau", "2026-06-14", "Imprimanta Termica", "Bixolon SRP-350", 3, 1200),
        S(32, "Lupu", "Cristian", "Franzeluta Magazin", "str. Botanica Veche 10, Chisinau", "2026-06-15", "Casa de Marcat", "Datecs DP-25", 1, 400)
    ];

    private static Solicitare S(int id, string nume, string prenume, string client, string adresa,
        string data, string tip, string model, int cant, decimal suma) => new()
    {
        IdSolicitare = id,
        NumeInginer = nume,
        PrenumeInginer = prenume,
        Client = client,
        Adresa = adresa,
        Data = DateTime.Parse(data),
        TipEchipament = tip,
        ModelEchipament = model,
        CantitateEchipament = cant,
        SumaAchitare = suma,
        Status = suma > 0
            ? StatusSolicitareMapper.ToStorage(StatusSolicitare.Finalizat)
            : StatusSolicitareMapper.ToStorage(StatusSolicitare.Preluat)
    };
}
