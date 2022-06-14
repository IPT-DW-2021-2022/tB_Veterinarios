using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Vets.Data;
using Vets.Models;

namespace Vets.Controllers.API {
   [Route("api/[controller]")]
   [ApiController]
   public class AnimaisAPIController : ControllerBase {
      private readonly ApplicationDbContext _context;

      public AnimaisAPIController(ApplicationDbContext context) {
         _context = context;
      }



      // GET: api/AnimaisAPI
      [HttpGet]
      public async Task<ActionResult<IEnumerable<AnimaisViewModel>>> GetAnimais() {
         return await _context.Animais
                              .Include(a => a.Dono)
                              .OrderByDescending(a=>a.Id)
                              .Select(a => new AnimaisViewModel {
                                 Id = a.Id,
                                 Nome = a.Nome,
                                 Especie = a.Especie,
                                 Raca = a.Raca,
                                 Peso = a.Peso,
                                 Fotografia = a.Fotografia,
                                 NomeDono = a.Dono.Nome
                              })
                             .ToListAsync();
      }



      // GET: api/AnimaisAPI/5
      [HttpGet("{id}")]
      public async Task<ActionResult<Animais>> GetAnimais(int id) {
         var animais = await _context.Animais.FindAsync(id);

         if (animais == null) {
            return NotFound();
         }

         return animais;
      }



      // PUT: api/AnimaisAPI/5
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPut("{id}")]
      public async Task<IActionResult> PutAnimais(int id, Animais animais) {
         if (id != animais.Id) {
            return BadRequest();
         }

         _context.Entry(animais).State = EntityState.Modified;

         try {
            await _context.SaveChangesAsync();
         }
         catch (DbUpdateConcurrencyException) {
            if (!AnimaisExists(id)) {
               return NotFound();
            }
            else {
               throw;
            }
         }

         return NoContent();
      }




      // POST: api/AnimaisAPI
      // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      [HttpPost]
      public async Task<ActionResult<Animais>> PostAnimais([FromForm] Animais animal, IFormFile uploadFotoAnimal) {

         // o anotador [FromForm] informa o ASP .NET que os dados são fornecidos 
         // em FormData

         /*
          * TAREFAS A EXECUTAR:
          * 1. validar os dados
          * 2. inserir a foto no disco rígido (semelhante ao feito no Veterinário)
          * 3. usar Try-Catch
          */


         animal.Fotografia = "noVet.jpg";


         // 3.
         try {
            _context.Animais.Add(animal);
            await _context.SaveChangesAsync();
         }
         catch (Exception) {

            throw;
         }


         return CreatedAtAction("GetAnimais", new { id = animal.Id }, animal);
      }




      // DELETE: api/AnimaisAPI/5
      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteAnimais(int id) {
         var animais = await _context.Animais.FindAsync(id);
         if (animais == null) {
            return NotFound();
         }

         _context.Animais.Remove(animais);
         await _context.SaveChangesAsync();

         return NoContent();
      }




      private bool AnimaisExists(int id) {
         return _context.Animais.Any(e => e.Id == id);
      }
   }
}
