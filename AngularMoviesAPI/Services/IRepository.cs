using AngularMoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.Services
{
    public interface IRepository
    {
        Task<List<Genre>> getAllGenres();
        Genre getGenreById(int Id);
        void addGenre(Genre genre);
    }



}
