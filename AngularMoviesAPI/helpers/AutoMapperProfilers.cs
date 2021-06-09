using AngularMoviesAPI.DTOs;
using AngularMoviesAPI.Entities;
using AutoMapper;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularMoviesAPI.helpers
{
    public class AutoMapperProfilers: Profile
    {
        public AutoMapperProfilers(GeometryFactory geometryFactory)
        {
            CreateMap<GenreDTO, Genre>().ReverseMap(); // GenreDTO mapped to Genre and verse versa (two ways mapping)
            CreateMap<GenreCreationDTO, Genre>(); // mapping genrecreationDTO to genre (one way mapping)

            // Actor mapping
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<ActorCreationDTO, Actor>()
                .ForMember(x => x.picture, options => options.Ignore()); // member setting

            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(x => x.latitude, dto => dto.MapFrom(prop => prop.location.Y))
                .ForMember(y => y.longitude, dto => dto.MapFrom(prop => prop.location.X));

            CreateMap<MovieTheaterCreationDTO, MovieTheater>()
                .ForMember(x => x.location, y => y.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.longitude, dto.latitude))));
            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(x => x.poster, options => options.Ignore())
                .ForMember(x => x.movieGenres, options => options.MapFrom(mapMovieGenres))
                .ForMember(x => x.movieActors, options => options.MapFrom(mapMovieActors))
                .ForMember(x => x.movieTheaterMovies, options => options.MapFrom(mapMovieTheaterMovies));

            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.movieTheaters, options => options.MapFrom(MapMovieTheatersMovies))
                .ForMember(x => x.actors, options => options.MapFrom(MapActors));
        }

        private List<MovieGenres> mapMovieGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        { 
            var result = new List<MovieGenres>();
            if (movieCreationDTO.genreIds == null) return result;

            foreach(var genreId in movieCreationDTO.genreIds)
            {
                result.Add(new MovieGenres() { genreId = genreId });
            }

            return result;
        }
        private List<MovieActors> mapMovieActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieActors>();
            if (movieCreationDTO.movieActors == null) return result;
            foreach(var actor in movieCreationDTO.movieActors)
            {
                result.Add(new MovieActors() { actorId = actor.id, character = actor.character });
            }
            return result;
        }
        private List<MovieTheaterMovies> mapMovieTheaterMovies(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MovieTheaterMovies>();
            if (movieCreationDTO.movietheaterIds == null) return result;
            foreach(var theaterId in movieCreationDTO.movietheaterIds)
            {
                result.Add(new MovieTheaterMovies() { movieTheaterId = theaterId});
            }
            return result;
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO moviedto)
        {
            var result = new List<GenreDTO>();
            if(movie.movieGenres != null)
            {
                foreach(var genre in movie.movieGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.genreId, Name = genre.Genre.Name});
                }
            }
            return result;
        }

        private List<MovieTheaterDTO> MapMovieTheatersMovies(Movie movie, MovieDTO moviedto)
        {
            var result = new List<MovieTheaterDTO>();
            if (movie.movieTheaterMovies != null)
            {
                foreach(var movieTheater in movie.movieTheaterMovies)
                {
                    result.Add(new MovieTheaterDTO()
                    {
                        id = movieTheater.movieTheaterId,
                        name = movieTheater.movieTheater.name,
                        latitude = movieTheater.movieTheater.location.X,
                        longitude = movieTheater.movieTheater.location.Y
                    });
                }
            }
            return result;
        }

        private List<ActorsMovieDTO> MapActors(Movie movie, MovieDTO moviedto)
        {
            var result = new List<ActorsMovieDTO>();
            if (movie.movieActors != null)
            {
                foreach(var actor in movie.movieActors)
                {
                    result.Add(new ActorsMovieDTO()
                    {
                        id = actor.actorId,
                        name = actor.actor.name,
                        character = actor.character,
                        picture = actor.actor.picture,
                        order = actor.order
                    });
                }
            }
            return result;
        }

    }
}
