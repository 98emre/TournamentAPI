using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto.TournamentDtos
{
    public class TournamentWithoutGamesDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate => StartDate.AddMonths(3);
    }
}
