using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tally_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 1. Nastavení Builderu
            var builder = WebApplication.CreateBuilder(args);

            // Registrace databáze a CORS
            builder.Services.AddDbContext<MojeDatabaze>(opt => 
                opt.UseSqlite("Data Source=tally.db"));
            
            builder.Services.AddCors(opt => 
                opt.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            // 2. Inicializace databáze (vytvoření tabulek a testovacího uživatele)
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MojeDatabaze>();
                db.Database.EnsureCreated();

                // Přidáme admina, pokud v databázi nikdo není
                if (!db.Uzivatele.Any())
                {
                    db.Uzivatele.Add(new Uzivatel { Jmeno = "admin", Heslo = "1234" });
                    db.SaveChanges();
                }
            }

            // 3. Middleware 
            app.UseCors();

            // 4. Endpointy (Adresy, na které server reaguje)
            app.MapPost("/prihlaseni", async (Uzivatel loginInfo, MojeDatabaze db) =>
            {
                // Hledáme uživatele v databázi podle jména a hesla
                var uzivatel = await db.Uzivatele.FirstOrDefaultAsync(u => 
                    u.Jmeno == loginInfo.Jmeno && u.Heslo == loginInfo.Heslo);

                if (uzivatel is null) 
                {
                    return Results.Unauthorized(); // Chyba 401
                }

                return Results.Ok(new { Zprava = "Vítej!", Uzivatel = uzivatel.Jmeno });
            });

            // Spuštění aplikace
            app.Run();
        }
    }

    // --- Datové Modely ---

    public class Uzivatel
    {
        public int Id { get; set; }
        public string Jmeno { get; set; } = string.Empty;
        public string Heslo { get; set; } = string.Empty; 
    }

    public class MojeDatabaze : DbContext
    {
        public MojeDatabaze(DbContextOptions<MojeDatabaze> options) : base(options) { }
        public DbSet<Uzivatel> Uzivatele => Set<Uzivatel>();
    }
}