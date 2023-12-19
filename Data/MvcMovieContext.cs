using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data
{
    public class MvcMovieContext : DbContext
    {
        // Como parâmetro, falamos que temos um contexto do banco de dados, e esse contexto será o MvcMovieContext, isso serve para a gente fazer o CRUD.
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        //O DBSet é uma representação do model Movie no BD
        public DbSet<MvcMovie.Models.Movie> Movie { get; set; }
    }
}
