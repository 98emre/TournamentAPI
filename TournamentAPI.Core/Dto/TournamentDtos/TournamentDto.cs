using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Dto.GameDtos;

namespace TournamentAPI.Core.Dto.TournamentDtos
{
    public class TournamentDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(3);

        public ICollection<GameWithIdDto> Games { get; set; } = new List<GameWithIdDto>();
    }
}
