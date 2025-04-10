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
            try
            {
                var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
                if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
                return Ok(games); //Tot bé al servidor -> retorna la llista de jocs
            }
            catch (Exception ex)
            {
                return BadRequest("Error al recuperar els jocs: " + ex.Message);
            }
        }
        // Per títol
        [HttpGet("titol")]
        public async Task<ActionResult<IEnumerable<Game>>> GetByName(string name)
        {
            try
            {
                var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
                if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
                var gamesWithName = games.Where(g => g.Title.ToLower().Contains(name.ToLower())); //Filtrant per nom
                if (gamesWithName.Count() == 0) { return NotFound("No hi ha cap joc amb aquest nom"); }
                return Ok(gamesWithName); //Tot bé al servidor -> retorna la llista de jocs
            }
            catch (Exception ex)
            {
                return BadRequest("Error al recuperar el joc: " + ex.Message);
            }
        }
        // Per equip desenvolupador
        [HttpGet("teamname")]
        public async Task<ActionResult<IEnumerable<Game>>> GetByTeam(string team)
        {
            try
            {
                var games = await _context.Games.ToListAsync(); //Obtenim tots els jocs de la base de dades
                if (games.Count == 0) { return NotFound("No hi ha cap joc"); }
                var gamesWithTeam = games.Where(g => g.TeamName.ToLower().Contains(team.ToLower())); //Filtrant per equip
                if (gamesWithTeam.Count() == 0) { return NotFound("No hi ha cap joc amb aquest nom"); }
                return Ok(gamesWithTeam); //Tot bé al servidor -> retorna la llista de jocs
            }
            catch (Exception ex)
            {
                return BadRequest("Error al recuperar el joc: " + ex.Message);
            }
        }

        // Inserció de jocs
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Game>> InsertGame(GameDTO gameInput) //https://youtu.be/TMWWpjFXiCc
        {
            if (gameInput != null) {
                try
                {
                    var game = new Game
                    {
                        Title = gameInput.Title,
                        Description = gameInput.Description,
                        TeamName = gameInput.TeamName
                    };
                    _context.Games.Add(game);
                    await _context.SaveChangesAsync();
                    return Ok($"Joc {game.Title} inserit!");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error en la inserció: {ex.Message}");
                }
            } 
            else
            {
                return BadRequest("No hi ha dades a inserir; el joc és buit!");
            }
        }

        // Edició de jocs

        // Eliminació de jocs

        // Votació

    }
}
