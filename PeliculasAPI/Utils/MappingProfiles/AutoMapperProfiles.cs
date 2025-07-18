﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.DTOs.GeneroDTOs;
using PeliculasAPI.DAL.DTOs.PeliculaDTOs;
using PeliculasAPI.DAL.DTOs.SalaDeCineDTOs;
using PeliculasAPI.DAL.DTOs.UsuarioDTOs;
using PeliculasAPI.DAL.Model;

namespace PeliculasAPI.Utils.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            CreateMap<IdentityUser, UsuarioDTO>();
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();

            CreateMap<SalaDeCine, SalaDeCineDTO>()
                .ForMember(x => x.Latitud, x => x.MapFrom(y => y.Ubicacion.Y))
                .ForMember(x => x.Longitud, x => x.MapFrom(y => y.Ubicacion.X));

            CreateMap<SalaDeCineDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));
                
            CreateMap<SalaDeCineCreacionDTO, SalaDeCine>()
                .ForMember(x => x.Ubicacion, x => x.MapFrom(y => geometryFactory.CreatePoint(new Coordinate(y.Longitud, y.Latitud))));

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.Generos, options => options.MapFrom(MapGeneroPelicula))
                .ForMember(x => x.Actores, options => options.MapFrom(MapActorPelicula));

            CreateMap<Pelicula, PeliculaDetallesDTO>()
                .ForMember(x => x.Generos, options => options.MapFrom(MapGenerosPeliculaCompleto))
                .ForMember(x => x.Actores, options => options.MapFrom(MapActoresPeliculaCompleto));
        }

        private List<GeneroDTO> MapGenerosPeliculaCompleto(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<GeneroDTO>();
            if (peliculaDetallesDTO.Generos == null)
            {
                return resultado;
            }

            foreach (var genero in pelicula.Generos)
            {
                resultado.Add(new GeneroDTO { Id = genero.GeneroId, Nombre = genero.Genero.Nombre});
            }
            return resultado;
        }

        private List<ActorPeliculaDetalleDTO> MapActoresPeliculaCompleto(Pelicula pelicula, PeliculaDetallesDTO peliculaDetallesDTO)
        {
            var resultado = new List<ActorPeliculaDetalleDTO>();
            if (peliculaDetallesDTO.Actores == null)
            {
                return resultado;
            }

            foreach (var actor in pelicula.Actores)
            {
                resultado.Add(new ActorPeliculaDetalleDTO { ActorId = actor.ActorId, Personaje = actor.Personaje, NombreActor = actor.Actor.Nombre });
            }
            return resultado;
        }

        private List<GeneroPelicula> MapGeneroPelicula(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<GeneroPelicula>();
            if (peliculaCreacionDTO.GenerosIds == null)
            {
                return resultado;
            }

            foreach (var id in peliculaCreacionDTO.GenerosIds)
            {
                resultado.Add(new GeneroPelicula { GeneroId = id });
            }
            return resultado;
        }

        private List<ActorPelicula> MapActorPelicula(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<ActorPelicula>();
            if (peliculaCreacionDTO.GenerosIds == null)
            {
                return resultado;
            }

            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new ActorPelicula { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return resultado;
        }
    }
}
