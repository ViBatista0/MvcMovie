using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;

namespace MvcMovie.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =new MvcMovieContext(serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
            {
                if (context.Movie.Any())
                {
                    return;
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "Moana",
                        ReleaseDate = DateTime.Parse("2016-11-11"),
                        Genre = "Animation",
                        Price = 7.99M,
                        Rating = "L"
                    },
                     new Movie
                     {
                         Title = "Madagascar",
                         ReleaseDate = DateTime.Parse("2013-05-20"),
                         Genre = "Animation",
                         Price = 7.99M,
                         Rating = "L"
                     },
                      new Movie
                      {
                          Title = "Kimi no Na Wa",
                          ReleaseDate = DateTime.Parse("2016-10-25"),
                          Genre = "Animation",
                          Price = 7.99M,
                          Rating = "12"

                      },
                       new Movie
                       {
                           Title = "Batman",
                           ReleaseDate = DateTime.Parse("2023-06-11"),
                           Genre = "Drama",
                           Price = 7.99M,
                           Rating = "14"
                       }
                );

                context.SaveChanges();
               
            }
        }
    }
}
