using AngularMoviesAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        // Add primary keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActors>().HasKey(x => new { x.actorId, x.movieId });
            modelBuilder.Entity<MovieGenres>().HasKey(x => new { x.movieId, x.genreId });
            modelBuilder.Entity<MovieTheaterMovies>().HasKey(x => new { x.movieTheaterId, x.movieId });

            base.OnModelCreating(modelBuilder);
        }
        // entity to table
        public DbSet<Genre> Genres {get;set;}
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MovieTheater { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<MovieActors> MovieActors {get;set;}
        public DbSet<MovieGenres> MovieGenres { get; set; }
        public DbSet<MovieTheaterMovies> MovieTheaterMovies { get; set; }
        // Package Manager Console
        // Add-Migration Actors
        // Update-database
    }
}
