using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcMovie.Models
{
    public class MovieGenreViewModel
    {
        //Lista de todos os filmes
        public List<Movie>? Movies { get; set; }

        //Uma lista que permite o usuário selecionar um dos items
        public SelectList? Genres { get; set; }

        //O genero que o usuário selecionar
        public string? MovieGenre { get; set; }

        //O filme que o usuário digitar
        public string? SearchString { get; set;}
    }
}
