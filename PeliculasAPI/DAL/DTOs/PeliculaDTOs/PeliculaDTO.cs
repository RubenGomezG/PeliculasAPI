﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.PeliculaDTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; }
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; }
    }
}
