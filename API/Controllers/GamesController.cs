using API.Data;
using API.DTOs;
using API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Consulta de jocs
        // ----------------
        // TOTS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetAll()
        {
            var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
            if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
            return Ok(games); //Tot bé al servidor -> retorna la llista de jocs
        }
        // Per títol
        [HttpGet("titol")]
        public async Task<ActionResult<IEnumerable<Game>>> GetByName(string name)
        {
            var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
            if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
            var gamesWithName = games.Where(g => g.Title.ToLower().Contains(name.ToLower())); //Filtrant per nom
            if (gamesWithName.Count() == 0) { return NotFound("No hi ha cap joc amb aquest nom"); }
            return Ok(gamesWithName); //Tot bé al servidor -> retorna la llista de jocs
        }
        // Per equip desenvolupador
        [HttpGet("teamname")]
        public async Task<ActionResult<IEnumerable<Game>>> GetByTeam(string team)
        {
            var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
            if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
            var gamesWithTeam = games.Where(g => g.TeamName.ToLower().Contains(team.ToLower())); //Filtrant per equip
            if (gamesWithTeam.Count() == 0) { return NotFound("No hi ha cap joc amb aquest nom"); }
            return Ok(gamesWithTeam); //Tot bé al servidor -> retorna la llista de jocs
        }

        // Inserció de jocs
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async void InsertGame(GameDTO gameInput)
        {
            var game = new Game
            {
                Title = gameInput.Title,
                Description = gameInput.Description,
                TeamName = gameInput.TeamName
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        // Edició de jocs

        // Eliminació de jocs

        // Votació

    }
}
