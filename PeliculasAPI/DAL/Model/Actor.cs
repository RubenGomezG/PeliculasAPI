﻿using PeliculasAPI.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.Model
{
    public class Actor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }

        public List<ActorPelicula> Peliculas { get; set; }
    }
}
