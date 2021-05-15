using AngularMoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Services
{
    public class InMemoryRepository: IRepository
    {
        private List<Genre> _genres;
        public InMemoryRepository()
        {
            this._genres = new List<Genre>
            {
                new Genre() {Id = 1, Name = "Comedy"},
                new Genre() {Id = 2, Name = "Action"}
            };
        }

        public async Task<List<Genre>> getAllGenres()
        {
            await Task.Delay(3);
            return _genres;
        }

        public Genre getGenreById(int Id) 
        {
            return _genres.FirstOrDefault(x => x.Id == Id); 
        }

        public void addGenre(Genre genre)
        {
            genre.Id = _genres.Max(x => x.Id) + 1;
            _genres.Add(genre);
        }
    }
}
