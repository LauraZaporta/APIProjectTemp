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
            if (gameInput == null) { return BadRequest("No hi ha dades a inserir; el joc és buit!"); }

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

        // Edició de jocs
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Game>> UpdateGame(GameDTO gameInput)
        {
            if (gameInput == null) { return BadRequest("No hi ha dades a inserir; el joc és buit!"); }

            try
            {
                var game = await _context.Games.FindAsync(gameInput.Title);
                if (game == null) { return NotFound("El joc a actualitzar no trobat"); }
                else
                {
                    game.Title = gameInput.Title;
                    game.Description = gameInput.Description;
                    game.TeamName = gameInput.TeamName;
                }
                await _context.SaveChangesAsync();
                return Ok($"Joc {game.Title} actualitzat!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error en la inserció: {ex.Message}");
            }
        }

        // Eliminació de jocs
        [Authorize(Roles = "Admin")]
        [HttpDelete]

        //Dintre del client fer un regex i que es pugui seleccionar amb el nom, una vegada trobat -> id a la funció.
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            try
            {
                var game = await _context.Games.FindAsync(id);
                if (game == null) { return NotFound("El joc a eliminar no trobat"); }
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
                return Ok($"Joc {game.Title} deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error en l'eliminació: {ex.Message}");
            }
        }

        // Votació

    }
}
