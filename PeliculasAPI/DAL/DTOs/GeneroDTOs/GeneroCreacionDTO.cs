using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.GeneroDTOs
{
    public class GeneroCreacionDTO
    {
        [Required]
        [StringLength(40)]
        public required string Nombre { get; set; }
    }
}
