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
using AutoMapper;
using TournamentAPI.Core.Dto;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();

            if(games.Count() == 0 || games == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {

            if (!await GameExists(id))
            {
                return NotFound();
            }

            var game = _mapper.Map<Game>(gameDto);
            game.Id = id;

            try
            {
                _unitOfWork.GameRepository.Update(game);
                await _unitOfWork.CompleteAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
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
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);

            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            var returnGame = _mapper.Map<GameDto>(game);

            return CreatedAtAction("GetGame", new { id = returnGame.Id }, returnGame);
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

        private async Task<bool> GameExists(int id) => await _unitOfWork.GameRepository.AnyAsync(id);
    }
}
