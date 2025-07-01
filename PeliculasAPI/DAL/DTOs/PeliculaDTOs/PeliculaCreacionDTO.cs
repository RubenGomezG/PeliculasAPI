using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Utils.Binders;
using PeliculasAPI.Utils.Enums;
using PeliculasAPI.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.PeliculaDTOs
{
    public class PeliculaCreacionDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }

        [PesoArchivoValidacion(4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculaCreacionDTO>>))]
        public List<ActorPeliculaCreacionDTO> Actores { get; set; }
    }
}
