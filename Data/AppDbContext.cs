using System.Linq.Expressions;
using EngineeringManagementApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EngineeringManagementApp.Data;

/// <summary>
/// Context Entity Framework pentru baza SQLite locală.
/// </summary>
public class AppDbContext : DbContext
{
    private readonly DatabaseConnection _databaseConnection;

    public AppDbContext(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
    }

    public DbSet<Solicitare> Solicitari => Set<Solicitare>();
    public DbSet<SolicitareAudit> SolicitariAudit => Set<SolicitareAudit>();
    public DbSet<Inginer> Ingineri => Set<Inginer>();
    public DbSet<Echipament> Echipamente => Set<Echipament>();
    public DbSet<Cont> Conturi => Set<Cont>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        optionsBuilder.UseSqlite(
            _databaseConnection.ConnectionString,
            opt => opt.CommandTimeout(_databaseConnection.CommandTimeoutSeconds));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cont>(entity =>
        {
            entity.HasIndex(c => c.Login).IsUnique();
            ConfigureNullableString(entity, c => c.Login);
            ConfigureNullableString(entity, c => c.Parola);
            ConfigureNullableString(entity, c => c.TipDrepturi);
        });

        modelBuilder.Entity<Echipament>(entity =>
        {
            entity.HasIndex(e => e.NrSerie).IsUnique();
            ConfigureNullableString(entity, e => e.TipEchipament);
            ConfigureNullableString(entity, e => e.ModelEchipament);
            ConfigureNullableString(entity, e => e.NrSerie);
        });

        modelBuilder.Entity<Inginer>(entity =>
        {
            ConfigureNullableString(entity, i => i.NumeInginer);
            ConfigureNullableString(entity, i => i.PrenumeInginer);
            entity.Property(i => i.CantitateEchipamenteDeservite)
                .HasConversion(new ValueConverter<int, int?>(v => v, v => v ?? 0));
        });

        modelBuilder.Entity<Solicitare>(entity =>
        {
            ConfigureNullableString(entity, s => s.NumeInginer);
            ConfigureNullableString(entity, s => s.PrenumeInginer);
            ConfigureNullableString(entity, s => s.Client);
            ConfigureNullableString(entity, s => s.Adresa);
            ConfigureNullableString(entity, s => s.TipEchipament);
            ConfigureNullableString(entity, s => s.ModelEchipament);
            ConfigureNullableString(entity, s => s.NrSerie);
            ConfigureNullableString(entity, s => s.Status);
            entity.Property(s => s.Data)
                .HasConversion(new ValueConverter<DateTime, DateTime?>(v => v, v => v ?? DateTime.MinValue));
            entity.Property(s => s.CantitateEchipament)
                .HasConversion(new ValueConverter<int, int?>(v => v, v => v ?? 0));
            entity.Property(s => s.SumaAchitare)
                .HasConversion(new ValueConverter<decimal, decimal?>(v => v, v => v ?? 0m));
        });

        modelBuilder.Entity<SolicitareAudit>(entity =>
        {
            ConfigureNullableString(entity, a => a.StatusVechi);
            ConfigureNullableString(entity, a => a.StatusNou);
            ConfigureNullableString(entity, a => a.Utilizator);
            entity.Property(a => a.DataSchimbare)
                .HasConversion(new ValueConverter<DateTime, DateTime?>(v => v, v => v ?? DateTime.MinValue));
        });
    }

    private static void ConfigureNullableString<TEntity>(
        EntityTypeBuilder<TEntity> entity,
        Expression<Func<TEntity, string>> propertyExpression) where TEntity : class
    {
        entity.Property(propertyExpression)
            .HasConversion(new ValueConverter<string, string>(v => v, v => v ?? string.Empty));
    }
}
