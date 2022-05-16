using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Vets.Models;

namespace Vets.Data {

   /// <summary>
   /// classe com os dados particulares do utilizador registado
   /// </summary>
   public class ApplicationUser : IdentityUser {

      /// <summary>
      /// nome de batismo do utilizador
      /// </summary>
      public string NomeDoUtilizador { get; set; }

      /// <summary>
      /// data em que o utilizador se registou
      /// </summary>
      public DateTime DataRegisto { get; set; }

   }




   /// <summary>
   /// esta classe funciona como a base de dados do nosso projeto
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options) {
      }


      /// <summary>
      /// este método é executado imediatamente antes 
      /// da criação do Modelo.
      /// É utilizado para adicionar as últimas instruções
      /// à criação do modelo
      /// </summary>
      /// <param name="modelBuilder"></param>
      protected override void OnModelCreating(ModelBuilder modelBuilder) {
         // 'importa' todo o comportamento do método, 
         // aquando da sua definição na SuperClasse
         base.OnModelCreating(modelBuilder);

         // adicionar registos que serão adicionados às
         // tabelas da BD
         modelBuilder.Entity<Veterinarios>().HasData(
            new Veterinarios() {
               Id = 1,
               Nome = "José Silva",
               NumCedulaProf = "vet-8765",
               Fotografia = "Jose.jpg"
            },
            new Veterinarios() {
               Id = 2,
               Nome = "Maria Gomes dos Santos",
               NumCedulaProf = "vet-6542",
               Fotografia = "Maria.jpg"
            },
            new Veterinarios() {
               Id = 3,
               Nome = "Ricardo Sousa",
               NumCedulaProf = "vet-1339",
               Fotografia = "Ricardo.jpg"
            }
         );

      }




      // definir as 'tabelas'
      public DbSet<Animais> Animais { get; set; }
      public DbSet<Veterinarios> Veterinarios { get; set; }
      public DbSet<Donos> Donos { get; set; }
      public DbSet<Consultas> Consultas { get; set; }




   }
}