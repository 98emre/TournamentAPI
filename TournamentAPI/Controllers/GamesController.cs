using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GamesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();

            if(games.Count() == 0 || games == null)
            {
                return NotFound();
            }

            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            var existingGame = await _unitOfWork.GameRepository.GetAsync(id);

            if (existingGame == null)
            {
                return NotFound();
            }

            existingGame.Title = game.Title;
            existingGame.Time = game.Time;
            existingGame.TournamentId = game.TournamentId;

            try
            {
                _unitOfWork.GameRepository.Update(existingGame);
                await _unitOfWork.CompleteAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExistsAsync(int id) => await _unitOfWork.GameRepository.AnyAsync(id);
    }
}
