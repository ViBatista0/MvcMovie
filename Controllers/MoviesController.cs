using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies, e se não tiver nenhum, irá exibir detalhes do problema
        public async Task<IActionResult> Index(string searchString, string movieGenre)
        {

            //Se não tiver nenhum contexto para o Movie, retorna nulo
            if (_context.Movie == null)
            {
                return Problem("Filme inexistente!");
            }

            //Criar uma consulta, em string dos gêneros do Movie, ordenados por gênero, não é executado no bd, apenas aqui.

            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            //Seleciona todos os filmes

            var movies = from m in _context.Movie
                         select m;

            //Se o usuário digitar na busca, então vai selecionar os filmes que tiverem os caracteres digitados (Contains)
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.Contains(searchString));
            }

            //Se o usuário selecionar um gênero, vai selecionar os filmes que tiverem o mesmo gênero.

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            //Aqui vai criar um SelectList, uma lista de seleção para todos os gêneros diferentes que existirem, e listar todos os filmes.

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Movies = await movies.ToListAsync()
            };

            //No fim de tudo, retornamos um MovieGenreViewModel, que exibe a lista dos filmes, e as opções de gêneros.
            return View(movieGenreVM);
        
        }
   


       //Mesmo que fazemos um get para fazer a busca, o botão ainda "envia" os dados, por isso aqui temos que colocar POST
       [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "Do [HttpPost]Index: esse foi o filtro buscado: " + searchString;
        }
        // GET: Movies/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /* Esse símbolo => indica que é uma expressão lambda, que funciona com uma função anônima, 
             assim não precisamos criar variáveis, dito que o "m" vai representar o "Movie". 
            O método FirstOrDefaultAsync retorna o primeiro caractere de uma sequência, caso o id passado
            na URL exista no contexto do Movie.
            */

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // Responsável por exibir o formulário para criar
        public IActionResult Create()
        {
            return View();
        }

        // Responsável por postar o formulário, ele usa o ModelState.IsValid para verificar se o filme tem erros de validação

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            return View(movie);
        }

        // Vai procurar o filme com o FindAsync, se não achar, retorna o 404
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // Bind protege contra o excesso de postagem, o ValidadeAntiForgeryToken protege contra um falsificação de uma
        // solicitação, por meio de um token criado na página de Edit, no <form asp-action="Edit">
        // ModelState.IsValid verifica se os dados passados pelo formulário são válidos para editar

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
       // Não deleta, mas retorna a view do formulário para confirmar a exclusão
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        // Realiza de fato a exclusão do item, o ActionName recebe o nome do método Delete, assim, no roteamento, o método DeleteConfirmed será encontrado
        // pela ULR acima.
   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
