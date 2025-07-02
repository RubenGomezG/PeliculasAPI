using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.DTOs.GeneroDTOs;

namespace PeliculasAPI.DAL.DTOs.PeliculaDTOs
{
    public class PeliculaDetallesDTO : PeliculaDTO
    {
        public List<GeneroDTO> Generos { get; set; }
        public List<ActorPeliculaDetalleDTO> Actores { get; set; }
    }
}
