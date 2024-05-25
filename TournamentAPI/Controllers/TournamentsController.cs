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
using AutoMapper;
using TournamentAPI.Core.Dto;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();

            if (tournaments == null || tournaments.Count() == 0)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournamentDto)
        {

            if (!await TournamentExists(id))
            {
                return NotFound();
            }

            var tournament = _mapper.Map<Tournament>(tournamentDto);
            tournament.Id = id;

            try
            {
                _unitOfWork.TournamentRepository.Update(tournament);
                await _unitOfWork.CompleteAsync();
            }
            
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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
        public async Task<ActionResult<TournamentDto>> PostTournament(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<Tournament>(tournamentDto);

            _unitOfWork.TournamentRepository.Add(tournament);
            await _unitOfWork.CompleteAsync();

            var returnTournament = _mapper.Map<TournamentDto>(tournament);

            return CreatedAtAction("GetTournament", new { id = returnTournament.Id }, returnTournament);
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

        private async Task<bool> TournamentExists(int id) => await _unitOfWork.TournamentRepository.AnyAsync(id);
    }
}
