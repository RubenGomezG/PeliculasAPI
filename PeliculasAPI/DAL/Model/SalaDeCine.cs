using NetTopologySuite.Geometries;
using PeliculasAPI.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.Model
{
    public class SalaDeCine : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }
        public List<PeliculasSalasDeCine> Peliculas { get; set; }
    }
}
