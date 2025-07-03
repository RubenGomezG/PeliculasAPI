using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.UsuarioDTOs
{
    public class UsuarioDTO
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
