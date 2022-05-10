using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Vets.Data;
using Vets.Models;

namespace Vets.Controllers {
   public class VeterinariosController : Controller {
      /// <summary>
      /// cria uma instancia de acesso à Base de Dados
      /// </summary>
      private readonly ApplicationDbContext _context;

      /// <summary>
      /// esta variável vai conter os dados do servidor
      /// </summary>
      private readonly IWebHostEnvironment _webHostEnvironment;

      public VeterinariosController(
         ApplicationDbContext context,
         IWebHostEnvironment webHostEnvironment) {
         _context = context;
         _webHostEnvironment = webHostEnvironment;
      }




      // GET: Veterinarios
      public async Task<IActionResult> Index() {
         /* acesso à base de dados
          * SELECT *
          * FROM Veterinários
          * 
          * e, depois estamos a enviar os dados para a View
          */

         return View(await _context.Veterinarios.ToListAsync());
      }





      // GET: Veterinarios/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await _context.Veterinarios
             .FirstOrDefaultAsync(m => m.Id == id);
         if (veterinarios == null) {
            return NotFound();
         }

         return View(veterinarios);
      }






      // GET: Veterinarios/Create
      /// <summary>
      /// usado para o primeiro acesso à View 'Create', em modo HTTP GET
      /// </summary>
      /// <returns></returns>
      public IActionResult Create() {
         return View();
      }





      // POST: Veterinarios/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// método usado para recuperar os dados enviados pelos utilizadores, 
      /// do Browser para o servidor
      /// </summary>
      /// <param name="veterinario"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create(
         [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario,
         IFormFile fotoVet) {
         /*
          * Algoritmo para processar ao ficheiro com a imagem
          * 
          * se ficheiro imagem nulo
          *    atribuir uma imagem genérica ao veterinário
          * else
          *    será que o ficheiro é uma imagem?
          *    se não for
          *       criar mensagem de erro
          *       devolver o controlo da app à view
          *    else
          *       - definir o nome a atribuir à imagem
          *       - atribuir aos dados do novo vet, o nome do ficheiro da imagem
          *       - guardar a imagem no disco rígido do servidor
          * */

         if (fotoVet == null) {
            veterinario.Fotografia = "noVet.png";
         }
         else {
            if (!(fotoVet.ContentType == "image/png" || fotoVet.ContentType == "image/jpeg")) {
               // criar mensagem de erro
               ModelState.AddModelError("", "Por favor, adicione um ficheiro .png ou .jpg");
               // revolver o controlo da app à View
               // fornecendo-lhe os dados que o utilizador já tinha preenchido no formulário
               return View(veterinario);
            }
            else {
               // temos ficheiro e é uma imagem...
               //++++++++++++++++++++++++++++++++
               // definir o nome da foto
               Guid g = Guid.NewGuid();
               string nomeFoto = veterinario.NumCedulaProf + "_" + g.ToString();
               string extensaoFoto = Path.GetExtension(fotoVet.FileName).ToLower();
               nomeFoto += extensaoFoto;
               // atribuir ao vet o nome da sua foto
               veterinario.Fotografia = nomeFoto;
            }
         }

         // avaliar se os dados fornecidos pelo utilizador
         // estão de acordo com as regras do Model
         if (ModelState.IsValid) {
            try {
               // adicionar os dados à BD
               _context.Add(veterinario);
               // consolidar esses dados (commit)
               await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
               // é da nossa responsabilidade!!! tratarmos da exceção

               // registar no disco rígido do servidor todos os dados da operação
               //     - data + hora
               //     - nome do utilizador
               //     - nome do controller + método
               //     - dados do erro (ex)
               //     - outros dados considerados úteis
               // eventualmente, tentar guardar na (numa) base de dados os dados do erro
               // eventualmente, notificar o Administrador da app do erro

               // no nosso caso,
               // criar uma msg de erro
               ModelState.AddModelError("", "Ocorreu um erro com a operação de guardar os dados do veterinário " + veterinario.Nome);
               // devolver controlo à View
               return View(veterinario);
            }

            //+++++++++++++++++++++++++++++++++++++++
            // concretizar a ação de guardar o ficheiro da foto
            //+++++++++++++++++++++++++++++++++++++++
            if (fotoVet != null) {
               // onde o ficheiro vai ser guardado?
               string nomeLocalizacaoFicheiro = _webHostEnvironment.WebRootPath;
               nomeLocalizacaoFicheiro = Path.Combine(nomeLocalizacaoFicheiro, "Fotos");
               // avaliar se a pasta 'Fotos' não existe
               if (!Directory.Exists(nomeLocalizacaoFicheiro)) {
                  Directory.CreateDirectory(nomeLocalizacaoFicheiro);
               }
               // nome do documento a guardar
               string nomeDaFoto = Path.Combine(nomeLocalizacaoFicheiro, veterinario.Fotografia);
               // criar o objeto que vai manipular o ficheiro
               using var stream = new FileStream(nomeDaFoto, FileMode.Create);
               // guardar no disco rígido
               await fotoVet.CopyToAsync(stream);
            }

            // devolver o controlo da app à View
            return RedirectToAction(nameof(Index));
         }
         return View(veterinario);
      }






      // GET: Veterinarios/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return RedirectToAction("Index");
         }

         var veterinario = await _context.Veterinarios.FindAsync(id);
         if (veterinario == null) {
            return RedirectToAction("Index");
         }

         // preservar, para memória futura, os dados que não devem
         // ser adulterados pelo utilizador no browser
         // vamos usar 'variáveis de sessão' (equivalentes a 'cookies')
         // podemos guardar INT e STRING
         // - quero guardar o ID do médico veterinário
         HttpContext.Session.SetInt32("VetID",veterinario.Id);

         return View(veterinario);
      }



      // POST: Veterinarios/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NumCedulaProf,Fotografia")] Veterinarios veterinario, IFormFile fotoVet) {
         if (id != veterinario.Id) {
            return NotFound();
         }

         /* confirmar se não houve adulteração de dados no browser
          * para que isto aconteça:
          * 1 - recuperar o valor da variável de sessão
          * 2 - comparar este valor com os dados que chegam do browser
          * 3 - se forem diferentes, temos um problema...
          */

         // (1)
         var idMedicoVeterinario = HttpContext.Session.GetInt32("VetID");

         /* se a variável 'idMedicoVeterinario' for nula,
         * o que aconteceu?
         *   - houve 'injeção' de dados através de uma ferramenta externa
         *   - demorou-se demasiado tempo na execução da tarefa
         */
         if (idMedicoVeterinario == null) {
            // neste caso o q fazer????
            ModelState.AddModelError("", "Demorou demasiado tempo a executar a tarefa de edição");
            return View(veterinario);
         }

         // (3)
         if (idMedicoVeterinario != veterinario.Id) {
            // temos problemas...
            // o que vamos fazer????
            return RedirectToAction("Index");
         }




         /*
          * só se altera a foto do Vet, se for carregado
          * algum ficheiro, e mesmo assim, só se for uma imagem
          * 
          * se há uma nova imagem, qual o seu nome?
          * vamos manter o nome da foto antiga ou dar um novo?
          * se mantiver, pode haver problemas com a cache do browser
          * se for novo, tenho de apagar o ficheiro antigo, 
          *     se não for o 'noVet.png'
          *     
          * Se o veterinário quiser voltar a usar a imagem
          * 'noVet.png' é necessário colocar essa pergunta 
          * na interface...
          * 
          */ 



         if (ModelState.IsValid) {
            try {
               _context.Update(veterinario);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!VeterinariosExists(veterinario.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(veterinario);
      }

      // GET: Veterinarios/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var veterinarios = await _context.Veterinarios
             .FirstOrDefaultAsync(m => m.Id == id);
         if (veterinarios == null) {
            return NotFound();
         }

         return View(veterinarios);
      }

      // POST: Veterinarios/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {

         try {
            var veterinario = await _context.Veterinarios.FindAsync(id);
            _context.Veterinarios.Remove(veterinario);
            await _context.SaveChangesAsync();

            // remover o ficheiro com a foto do Veterinário
            // se a foto NÃO FOR a 'noVet.png'
         }
         catch (Exception) {
            //   throw;
            // não esquecer, tratar da exceção
         }


         return RedirectToAction(nameof(Index));
      }

      private bool VeterinariosExists(int id) {
         return _context.Veterinarios.Any(e => e.Id == id);
      }
   }
}
