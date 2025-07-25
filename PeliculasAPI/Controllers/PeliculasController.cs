﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.BLL.Services.Interfaces;
using PeliculasAPI.DAL;
using PeliculasAPI.DAL.DTOs;
using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.DTOs.PeliculaDTOs;
using PeliculasAPI.DAL.Model;
using PeliculasAPI.Utils.Extensions;
using System.Linq.Dynamic.Core;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly ILogger<PeliculasController> _logger;
        private readonly string contenedor = "peliculas";

        public PeliculasController(ApplicationDbContext context,
            IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos,
            ILogger<PeliculasController> logger)
            : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            return await Get<Pelicula, PeliculaDTO>(paginacionDTO);
        }

        [HttpGet]
        public async Task<ActionResult<PeliculasIndexDTO>> Get()
        {
            var top = 5;
            var hoy = DateTime.Today;
            var proximosEstrenos = await _context.Peliculas
                .Where(x => x.FechaEstreno > hoy)
                .OrderBy(x => x.FechaEstreno)
                .Take(top)
                .ToListAsync();

            var enCines = await _context.Peliculas
                .Where(x => x.EnCines)
                .Take(top)
                .ToListAsync();

            var resultado = new PeliculasIndexDTO
            {
                ProximosEstrenos = _mapper.Map<List<PeliculaDTO>>(proximosEstrenos),
                EnCines = _mapper.Map<List<PeliculaDTO>>(enCines)
            };

            return resultado;
        }

        [HttpGet("filtro")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculasDTO filtroPeliculasDTO)
        {
            IQueryable<Pelicula> queryable = _context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(filtroPeliculasDTO.Titulo))
            {
                queryable = queryable.Where(x => x.Titulo.Contains(filtroPeliculasDTO.Titulo));
            }

            if (filtroPeliculasDTO.EnCines)
            {
                queryable = queryable.Where(x => x.EnCines);
            }

            if (filtroPeliculasDTO.PorEstrenar)
            {
                queryable = queryable.Where(x => x.FechaEstreno > DateTime.Today);
            }

            if (_context.Generos.Any(x => x.Id == filtroPeliculasDTO.GeneroId))
            {
                queryable = queryable
                    .Where(x => x.Generos.Select(y => y.GeneroId)
                    .Contains(filtroPeliculasDTO.GeneroId));
            }

            if (!string.IsNullOrEmpty(filtroPeliculasDTO.CampoOrdenacion))
            {
                var tipoOrden = filtroPeliculasDTO.OrdenAscendente ? "ascending" : "descending";
                try
                {
                    queryable = queryable.OrderBy($"{filtroPeliculasDTO.CampoOrdenacion} {tipoOrden}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                
            }

            await HttpContext.InsertarParametrosPaginacion(queryable, filtroPeliculasDTO.Paginacion.CantidadRegistrosPorPagina);
            List<Pelicula> peliculas = await queryable.Paginar(filtroPeliculasDTO.Paginacion).ToListAsync();
            return _mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        [HttpGet("{id:int}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDetallesDTO>> Get(int id)
        {
            Pelicula pelicula = await _context.Peliculas
                .Include(x => x.Actores)
                .ThenInclude(y => y.Actor)
                .Include(x => x.Generos)
                .ThenInclude(y => y.Genero)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula.Actores = pelicula.Actores.OrderBy(actores => actores.Orden).ToList();
            return _mapper.Map<PeliculaDetallesDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            Pelicula pelicula = _mapper.Map<Pelicula>(peliculaCreacionDTO);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await _almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            AsignarOrdenActores(pelicula);
            _context.Add(pelicula);
            await _context.SaveChangesAsync();
            PeliculaDTO peliculaDto = _mapper.Map<PeliculaDTO>(pelicula);
            return CreatedAtRoute("obtenerActor", new { id = peliculaDto.Id }, peliculaDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, PeliculaCreacionDTO peliculaCreacionDTO)
        {
            Pelicula pelicula = await _context.Peliculas
                .Include(x => x.Actores)
                .Include(x => x.Generos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            pelicula = _mapper.Map(peliculaCreacionDTO, pelicula);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await _almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, pelicula.Poster, peliculaCreacionDTO.Poster.ContentType);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            return await Delete<Pelicula>(id);
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if (pelicula.Actores != null)
            {
                for (int i = 0; i < pelicula.Actores.Count; i++)
                {
                    pelicula.Actores[i].Orden = i;
                }
            }
        }
    }
}
