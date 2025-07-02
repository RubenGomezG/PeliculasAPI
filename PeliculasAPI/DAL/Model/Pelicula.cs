using PeliculasAPI.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.Model
{
    public class Pelicula : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }
        public List<ActorPelicula> Actores { get; set; }
        public List<GeneroPelicula> Generos { get; set; }
        public List<PeliculasSalasDeCine> SalasDeCine { get; set; }
    }
}
