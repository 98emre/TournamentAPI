﻿using System;
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
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using TournamentAPI.Core.Dto.TournamentDtos;
using TournamentDto = TournamentAPI.Core.Dto.TournamentDtos.TournamentDto;

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

        [HttpGet]
        public async Task<IActionResult> GetTournament(bool includeGames = true)
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync(includeGames);

            if (tournaments == null || tournaments.Count() == 0)
            {
                return NotFound();
            }

            return Ok(includeGames ? _mapper.Map<IEnumerable<TournamentDto>>(tournaments) : _mapper.Map<IEnumerable<TournamentWithoutGamesDto>>(tournaments));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournament(int id, bool includeGames = true)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id, includeGames);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(includeGames ? _mapper.Map<TournamentDto>(tournament) : _mapper.Map<TournamentWithoutGamesDto>(tournament));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentPostDto tournamentDto)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            if (!await TournamentExists(id))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the tournament.");
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TournamentPostDto>> PostTournament(TournamentPostDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournament = _mapper.Map<Tournament>(tournamentDto);

            try
            {
                _unitOfWork.TournamentRepository.Add(tournament);
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while posting the tournament.");
            }

            var returnTournament = _mapper.Map<TournamentDto>(tournament);

            return CreatedAtAction("GetTournament", new { id = returnTournament.Id }, returnTournament);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id,includeGames: false);

            if (tournament == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.TournamentRepository.Remove(tournament);
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the tournament.");
            }

            return NoContent();
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentPostDto>> PatchTournament(int tournamentId, JsonPatchDocument<TournamentPostDto> patchDocument)
        {
            if (tournamentId <= 0)
            {
                return BadRequest();
            }

            if (patchDocument == null)
            {
                return BadRequest();
            }

            var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId, includeGames: true);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentToPatch = _mapper.Map<TournamentPostDto>(tournament);
            patchDocument.ApplyTo(tournamentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(tournament))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(tournamentToPatch, tournament);
            
            try
            {
                _unitOfWork.TournamentRepository.Update(tournament);
                await _unitOfWork.CompleteAsync();
            }

            catch (Exception)
            {
                return StatusCode(500, "An error occurred while patching the tournament.");
            }

            return NoContent();
        }

        private async Task<bool> TournamentExists(int id) => await _unitOfWork.TournamentRepository.AnyAsync(id);
    }
}
