namespace EngineeringManagementApp.Services;

/// <summary>
/// Utilitar linie de comandă: resetează parola unui cont fără a deschide interfața grafică.
/// Utilizare: EngineeringManagementApp.exe --reset-parola &lt;login&gt; &lt;parola_noua&gt;
/// </summary>
public static class PasswordResetCli
{
    public static bool IncearcaExecutare(string[] args)
    {
        if (args.Length < 3 || !string.Equals(args[0], "--reset-parola", StringComparison.OrdinalIgnoreCase))
            return false;

        var login = args[1];
        var parolaNoua = args[2];

        try
        {
            AppServices.Initialize();
            AppServices.DatabaseInitializer.InitializeAsync().GetAwaiter().GetResult();

            var serviciu = new ConturiService();
            serviciu.ReseteazaParolaAsync(login, parolaNoua).GetAwaiter().GetResult();

            Console.WriteLine($"Parola contului '{login}' a fost resetată cu succes.");
            Console.WriteLine($"Baza de date: {AppServices.DatabaseConnection.CaleFisierBazaDate}");
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Eroare la resetarea parolei: {ex.Message}");
            return true;
        }
        finally
        {
            AppServices.Dispose();
        }
    }
}
