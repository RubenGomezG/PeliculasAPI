﻿using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.DAL.DTOs.UsuarioDTOs
{
    public class UserInfo
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
