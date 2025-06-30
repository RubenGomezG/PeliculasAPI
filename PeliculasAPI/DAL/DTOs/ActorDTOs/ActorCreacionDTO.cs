using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.ActorDTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public required string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
