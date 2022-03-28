using System.ComponentModel.DataAnnotations.Schema;

namespace Vets.Models {
   public class Animais {

      public Animais() {
         ListaConsultas = new HashSet<Consultas>();
      }

      public int Id { get; set; }

      public string Nome { get; set; }

      public string Raca { get; set; }

      public string Especie { get; set; }

      public DateTime DataNascimento { get; set; }

      public double Peso { get; set; }

      public string Fotografia { get; set; }


      [ForeignKey(nameof(Dono))]  //   [ForeignKey("Dono")]
      public int DonoFK { get; set; }
      public Donos Dono { get; set; }


      public ICollection<Consultas> ListaConsultas { get; set; }

   }
}
