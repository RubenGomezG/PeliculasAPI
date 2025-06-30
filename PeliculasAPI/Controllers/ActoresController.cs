using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.DAL;
using PeliculasAPI.DAL.DTOs.ActorDTOs;
using PeliculasAPI.DAL.Model;

namespace PeliculasAPI.Controllers
{
    [ApiController]
    [Route("api/actores")]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            List<Actor> entidades = await _context.Actores.ToListAsync();
            return _mapper.Map<List<ActorDTO>>(entidades);
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
        public async Task<ActionResult> Post([FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            Actor actor = _mapper.Map<Actor>(actorCreacionDTO);
            _context.Add(actor);
            await _context.SaveChangesAsync();
            ActorDTO actorDto = _mapper.Map<ActorDTO>(actor);
            return CreatedAtRoute("obtenerActor", new { id = actorDto.Id }, actorDto);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            Actor actor = _mapper.Map<Actor>(actorCreacionDTO);
            actor.Id = id;
            _context.Entry(actor).State = EntityState.Modified;
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
