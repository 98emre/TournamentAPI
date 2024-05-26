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
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using TournamentAPI.Core.Dto.GameDtos;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame(string? filterTitle = null, bool sort = false)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync(filterTitle, sort);

            if(!games.Any() || games == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GamePostDto gameDto)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await GameExists(id))
            {
                return NotFound();
            }

            if (!(await TournamentExists(gameDto.TournamentId))) { 
                return BadRequest($"Tournament with Id {gameDto.TournamentId} not found");
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

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the game.");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GamePostDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!(await TournamentExists(gameDto.TournamentId)))
            {
                return BadRequest($"Tournament with Id {gameDto.TournamentId} not found");
            }

            var game = _mapper.Map<Game>(gameDto);

            try
            {
                _unitOfWork.GameRepository.Add(game);
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while posting the game.");
            }

            var returnGame = _mapper.Map<GameDto>(game);

            return CreatedAtAction("GetGame", new { id = returnGame.Id }, returnGame);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.GameRepository.Remove(game);
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the game.");

            }

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public async Task<IActionResult> PatchGame(int gameId, JsonPatchDocument<GamePostDto> patchDocument)
        {
            if(gameId <= 0)
            {
                return BadRequest();
            }

            if (patchDocument == null)
            {
                return BadRequest();
            }

            var game = await _unitOfWork.GameRepository.GetAsync(gameId);

            if (game == null)
            {
                return NotFound();
            }

            var gameToPatch = _mapper.Map<GamePostDto>(game);
            patchDocument.ApplyTo(gameToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(game))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameToPatch, game);

            try
            {
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while patch the game.");
            }

            return NoContent();
        }

        private async Task<bool> GameExists(int id) => await _unitOfWork.GameRepository.AnyAsync(id);

        private async Task<bool> TournamentExists(int id) => await _unitOfWork.TournamentRepository.AnyAsync(id);
    }
}
