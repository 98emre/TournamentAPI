using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto.GameDtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Time { get; set; }

        public int TournamentId { get; set; }

    }
}
