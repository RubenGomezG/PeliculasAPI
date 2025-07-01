using PeliculasAPI.Utils.Enums;
using PeliculasAPI.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.ActorDTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidacion(pesoMaximoEnMegaBytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public IFormFile Foto { get; set; }
    }
}
