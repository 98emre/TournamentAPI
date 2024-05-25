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
using TournamentAPI.Data.Repositories;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TournamentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();

            if (tournaments == null || tournaments.Count() == 0)
            {
                return NotFound();
            }


            return Ok(tournaments);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            var existTournament = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (existTournament == null)
            {
                return NotFound();
            }

            existTournament.Title = tournament.Title;
            existTournament.StartDate = tournament.StartDate;
            existTournament.Games = tournament.Games;

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
        {
            _unitOfWork.TournamentRepository.Add(tournament);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetTournament", new { id = tournament.Id }, tournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        private bool TournamentExists(int id)
        {
            var tournament = _unitOfWork.TournamentRepository.AnyAsync(id);

            return (tournament == null) ? false : true;
        }
    }
}
