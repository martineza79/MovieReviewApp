using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieReviewApp.Models
{
    public class Movie
    {
        public int id { get; set; } 

        public string title { get; set; }

        public string contentRating { get; set; }

        public List<Movie> GetAll()
        {
            List<Movie> lstMovie = new List<Movie>()
            {
                new Movie { id = 1, title = "Minions", contentRating = "PG"},
                new Movie { id = 2, title = "Rocky", contentRating = "PG"},
                new Movie { id = 3, title = "Monsters Inc", contentRating = "PG" }
            };

            return lstMovie;
        }

        public Movie GetMovie(int id)
        {
            return GetAll().Where(x => x.id == id).FirstOrDefault();
        }

        public List<Movie> Create(Movie movie)
        {
            List<Movie> lstMovies = GetAll();
            lstMovies.Add(movie);
            return lstMovies;
        }

        public List<Movie> Update (Movie movie)
        {
            List<Movie> lstMovies = GetAll().Where(x => x.id != movie.id).ToList();

            Movie m = GetMovie(movie.id);
            lstMovies.Add(movie);
            return lstMovies;
        }

        public List<Movie> Delete(int id)
        {
            List<Movie> lstMovies = GetAll().Where(x => x.id != id).ToList();

            Movie movie = GetMovie(id);
            return lstMovies;
        }
    }
}