using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vets.Models {
   public class Consultas {

      public int Id { get; set; }

      [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                     ApplyFormatInEditMode = true)]
      [DataType(DataType.Date)]
      public DateTime Data { get; set; }

      public string Observacoes { get; set; }

      public decimal ValorConsulta { get; set; }


      [ForeignKey(nameof(Animal))]
      public int AnimalFK { get; set; }
      public Animais Animal { get; set; }


      [ForeignKey(nameof(Veterinario))]
      public int VeterinarioFK { get; set; }
      public Veterinarios Veterinario { get; set; }

   }
}
