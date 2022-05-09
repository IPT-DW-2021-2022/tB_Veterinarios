using System.ComponentModel.DataAnnotations;

namespace Vets.Models {
   /// <summary>
   /// modelo que interage com os dados dos Veterinário
   /// </summary>
   public class Veterinarios {

      public Veterinarios() {
         ListaConsultas=new HashSet<Consultas>();
      }


      /// <summary>
      /// PK para cada um dos resgistos da tabela
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Nome do Veterinário
      /// </summary>
      public string Nome { get; set; }

      /// <summary>
      /// Nº da Cédula Profissional
      /// </summary>
      [Display(Name ="Nºcédula profissional")]
      public string NumCedulaProf { get; set; }

      /// <summary>
      /// Nome do ficheiro que contém a fotografia do Veterinário
      /// </summary>
      public string Fotografia { get; set; }


      /// <summary>
      /// Lista de Consulta feitas pelo Veterinário
      /// </summary>
      public ICollection<Consultas> ListaConsultas { get; set; }
   }
}
