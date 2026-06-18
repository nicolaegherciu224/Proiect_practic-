using EngineeringManagementApp.Data;
using EngineeringManagementApp.Services;
using Microsoft.Extensions.Configuration;

namespace EngineeringManagementApp;

/// <summary>
/// Application-wide service locator for shared infrastructure instances.
/// Keeps startup wiring simple until a full DI container is introduced in later steps.
/// </summary>
public static class AppServices
{
  private static IConfiguration? _configuration;
  private static DatabaseConnection? _databaseConnection;
  private static AppDbContext? _dbContext;
  private static AuthService? _authService;
  private static DatabaseInitializer? _databaseInitializer;

  public static IConfiguration Configuration =>
    _configuration ?? throw new InvalidOperationException("Application services are not initialized.");

  public static DatabaseConnection DatabaseConnection =>
    _databaseConnection ?? throw new InvalidOperationException("Application services are not initialized.");

  public static AppDbContext DbContext =>
    _dbContext ?? throw new InvalidOperationException("Application services are not initialized.");

  /// <summary>
  /// Creează un context nou pentru operații CRUD (evită conflicte de tracking EF).
  /// </summary>
  public static AppDbContext CreazaContext() =>
    new AppDbContext(DatabaseConnection);

  public static AuthService Auth =>
    _authService ?? throw new InvalidOperationException("Application services are not initialized.");

  public static DatabaseInitializer DatabaseInitializer =>
    _databaseInitializer ?? throw new InvalidOperationException("Application services are not initialized.");

  public static void Initialize()
  {
    _configuration = new ConfigurationBuilder()
      .SetBasePath(AppContext.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .Build();

    _databaseConnection = new DatabaseConnection(_configuration);
    _dbContext = new AppDbContext(_databaseConnection);
    _authService = new AuthService(_dbContext);
    _databaseInitializer = new DatabaseInitializer(_dbContext, _databaseConnection);
  }

  public static void Dispose()
  {
    _dbContext?.Dispose();
    _dbContext = null;
    _authService = null;
    _databaseInitializer = null;
    _databaseConnection = null;
    _configuration = null;
  }
}
