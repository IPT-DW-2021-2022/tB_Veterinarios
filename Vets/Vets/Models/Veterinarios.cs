namespace Vets.Models {
   public class Veterinarios {

      public Veterinarios() {
         ListaConsultas=new HashSet<Consultas>();
      }


      public int Id { get; set; }

      public string Nome { get; set; }

      public string NumCedulaProf { get; set; }

      public string Fotografia { get; set; }


      public ICollection<Consultas> ListaConsultas { get; set; }
   }
}
