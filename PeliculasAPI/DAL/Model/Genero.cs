using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.Model
{
    public class Genero
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public required string Nombre { get; set; }
    }
}
