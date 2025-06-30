using AutoMapper;
using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.DTOs.GeneroDTOs;
using PeliculasAPI.DAL.Model;

namespace PeliculasAPI.Utils.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>();
        }
    }
}
