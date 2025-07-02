﻿using PeliculasAPI.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.Model
{
    public class Genero : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }
        public List<GeneroPelicula> Peliculas { get; set; }
    }
}
