namespace Vets.Models {
   public class Donos {

      public Donos() {
         ListaAnimais = new HashSet<Animais>();
      }

      public int Id { get; set; }

      public string Nome { get; set; }

      public string NIF { get; set; }

      public string Sexo { get; set; }


      public ICollection<Animais> ListaAnimais { get; set; }

   }
}
