namespace Vets.Models {

   /// <summary>
   /// ViewModel para ser usado na API dos Donos
   /// </summary>
   public class DonosViewModel {
      public int Id { get; set; }
      public string Nome { get; set; }
   }


   /// <summary>
   /// Este viewmodel irá recolher os dados dos Animais que a API 
   /// irá apresentar na sua interface
   /// </summary>
   public class AnimaisViewModel {
      public int Id { get; set; }
      public string Nome { get; set; }
      public string Raca { get; set; }
      public string Especie { get; set; }
      public double Peso { get; set; }
      public string Fotografia { get; set; }
      public string NomeDono { get; set; }
   }





   public class ErrorViewModel {
      public string RequestId { get; set; }

      public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
   }
}