using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DAL;
using PeliculasAPI.DAL.DTOs.GeneroDTOs;
using PeliculasAPI.DAL.Model;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            List<Genero> entidades = await _context.Generos.ToListAsync();
            List<GeneroDTO> dtos = _mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "obtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            Genero genero = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
            {
                return NotFound();
            }

            GeneroDTO dto = _mapper.Map<GeneroDTO>(genero);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            Genero genero = _mapper.Map<Genero>(generoCreacionDTO);
            _context.Add(genero);
            await _context.SaveChangesAsync();
            var generoDTO = _mapper.Map<GeneroDTO>(genero);

            return CreatedAtRoute("obtenerGenero", new { id = generoDTO.Id}, generoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, GeneroCreacionDTO generoDTO)
        {
            if (!_context.Generos.Any(x => x.Id == id))
            {
                return NotFound();
            }

            Genero genero = _mapper.Map<Genero>(generoDTO);
            genero.Id = id;
            _context.Entry(genero).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!_context.Generos.Any(x => x.Id == id))
            {
                return NotFound();
            }

            _context.Remove(new Genero() { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
