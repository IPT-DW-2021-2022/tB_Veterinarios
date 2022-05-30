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

      /// <summary>
      /// este atributo serve para auxiliar à introdução
      /// de dados no atributo 'decimal'
      /// </summary>
      [NotMapped] // indica à EF que não deve representar este atributo na base de dados
      [Required(ErrorMessage = "Preencha, pf, o valor da consulta")]
      [StringLength(11)]
      [RegularExpression("[0-9]{1,8}[,.]?[0-9]{0,2}",
                          ErrorMessage = "Indique, por favor, o valor da consulta...")]
      public string AuxValorConsulta { get; set; }


      public decimal ValorConsulta { get; set; }


      [ForeignKey(nameof(Animal))]
      public int AnimalFK { get; set; }
      public Animais Animal { get; set; }


      [ForeignKey(nameof(Veterinario))]
      public int VeterinarioFK { get; set; }
      public Veterinarios Veterinario { get; set; }

   }
}
