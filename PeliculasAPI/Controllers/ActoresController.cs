using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.BLL.Services.Interfaces;
using PeliculasAPI.DAL;
using PeliculasAPI.DAL.DTOs;
using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.Model;
using PeliculasAPI.Utils.Extensions;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorArchivos _almacenadorArchivos;
        private readonly string contenedor = "actores";

        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            _context = context;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            IQueryable<Actor> queryable = _context.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacion(queryable, paginacionDTO.CantidadRegistrosPorPagina);
            List<Actor> actores = await queryable.Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpGet("{id:int}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            Actor actor = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }

            return _mapper.Map<ActorDTO>(actor);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            Actor actor = _mapper.Map<Actor>(actorCreacionDTO);
            if (actorCreacionDTO.Foto != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actor.Foto = await _almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor, actorCreacionDTO.Foto.ContentType);
                }
            }
            _context.Add(actor);
            await _context.SaveChangesAsync();
            ActorDTO actorDto = _mapper.Map<ActorDTO>(actor);
            return CreatedAtRoute("obtenerActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            Actor actor = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if(actor == null)
            {
                return NotFound();
            }

            actor = _mapper.Map(actorCreacionDTO, actor);
            if (actorCreacionDTO.Foto != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actor.Foto = await _almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, actor.Foto, actorCreacionDTO.Foto.ContentType);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_context.Actores.Any(x => x.Id == id))
            {
                return NotFound();
            }

            _context.Remove(new Actor() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
