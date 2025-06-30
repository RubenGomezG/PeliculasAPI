using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DAL.Model;

namespace PeliculasAPI.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }
}
