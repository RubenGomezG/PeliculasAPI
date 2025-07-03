using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPI.DAL.Model;
using System.Security.Claims;

namespace PeliculasAPI.DAL
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        private void SeedData(ModelBuilder modelBuilder)
        {

            string rolAdminId = "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d";
            string usuarioAdminId = "5673b8cf-12de-44f6-92ad-fae4a77932ad";

            IdentityRole rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();

            string username = "felipe@hotmail.com";

            IdentityUser usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,
                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            };

            GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            modelBuilder.Entity<SalaDeCine>()
               .HasData(new List<SalaDeCine>
               {
                    new SalaDeCine{Id = 1, Nombre = "Agora", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.9388777, 18.4839233))},
                    new SalaDeCine{Id = 4, Nombre = "Sambil", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.9118804, 18.4826214))},
                    new SalaDeCine{Id = 5, Nombre = "Megacentro", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-69.856427, 18.506934))},
                    new SalaDeCine{Id = 6, Nombre = "Village East Cinema", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-73.986227, 40.730898))}
               });

            Genero aventura = new Genero() { Id = 4, Nombre = "Aventura" };
            Genero animation = new Genero() { Id = 5, Nombre = "Animación" };
            Genero suspenso = new Genero() { Id = 6, Nombre = "Suspenso" };
            Genero romance = new Genero() { Id = 7, Nombre = "Romance" };

            modelBuilder.Entity<Genero>()
                .HasData(new List<Genero>
                {
                    aventura, animation, suspenso, romance
                });

            Actor jimCarrey = new Actor() { Id = 5, Nombre = "Jim Carrey", FechaNacimiento = new DateTime(1962, 01, 17) };
            Actor robertDowney = new Actor() { Id = 6, Nombre = "Robert Downey Jr.", FechaNacimiento = new DateTime(1965, 4, 4) };
            Actor chrisEvans = new Actor() { Id = 7, Nombre = "Chris Evans", FechaNacimiento = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            Pelicula endgame = new Pelicula()
            {
                Id = 2,
                Titulo = "Avengers: Endgame",
                EnCines = true,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            Pelicula iw = new Pelicula()
            {
                Id = 3,
                Titulo = "Avengers: Infinity Wars",
                EnCines = false,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            Pelicula sonic = new Pelicula()
            {
                Id = 4,
                Titulo = "Sonic the Hedgehog",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 02, 28)
            };

            Pelicula emma = new Pelicula()
            {
                Id = 5,
                Titulo = "Emma",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 02, 21)
            };

            Pelicula wonderwoman = new Pelicula()
            {
                Id = 6,
                Titulo = "Wonder Woman 1984",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Pelicula>()
                .HasData(new List<Pelicula>
                {
                    endgame, iw, sonic, emma, wonderwoman
                });

            modelBuilder.Entity<GeneroPelicula>().HasData(
                new List<GeneroPelicula>()
                {
                    new GeneroPelicula(){PeliculaId = endgame.Id, GeneroId = suspenso.Id},
                    new GeneroPelicula(){PeliculaId = endgame.Id, GeneroId = aventura.Id},
                    new GeneroPelicula(){PeliculaId = iw.Id, GeneroId = suspenso.Id},
                    new GeneroPelicula(){PeliculaId = iw.Id, GeneroId = aventura.Id},
                    new GeneroPelicula(){PeliculaId = sonic.Id, GeneroId = aventura.Id},
                    new GeneroPelicula(){PeliculaId = emma.Id, GeneroId = suspenso.Id},
                    new GeneroPelicula(){PeliculaId = emma.Id, GeneroId = romance.Id},
                    new GeneroPelicula(){PeliculaId = wonderwoman.Id, GeneroId = suspenso.Id},
                    new GeneroPelicula(){PeliculaId = wonderwoman.Id, GeneroId = aventura.Id},
                });

            modelBuilder.Entity<ActorPelicula>().HasData(
                new List<ActorPelicula>()
                {
                    new ActorPelicula(){PeliculaId = endgame.Id, ActorId = robertDowney.Id, Personaje = "Tony Stark", Orden = 1},
                    new ActorPelicula(){PeliculaId = endgame.Id, ActorId = chrisEvans.Id, Personaje = "Steve Rogers", Orden = 2},
                    new ActorPelicula(){PeliculaId = iw.Id, ActorId = robertDowney.Id, Personaje = "Tony Stark", Orden = 1},
                    new ActorPelicula(){PeliculaId = iw.Id, ActorId = chrisEvans.Id, Personaje = "Steve Rogers", Orden = 2},
                    new ActorPelicula(){PeliculaId = sonic.Id, ActorId = jimCarrey.Id, Personaje = "Dr. Ivo Robotnik", Orden = 1}
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActorPelicula>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<GeneroPelicula>()
                .HasKey(x => new { x.GeneroId, x.PeliculaId });

            modelBuilder.Entity<PeliculasSalasDeCine>()
                .HasKey(x => new { x.PeliculaId, x.SalaDeCineId});

            SeedData(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<ActorPelicula> ActoresPeliculas { get; set; }
        public DbSet<GeneroPelicula> GenerosPeliculas { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
    }
}
