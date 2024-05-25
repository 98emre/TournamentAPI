using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = default!;

        public DateTime Time { get; set; }

        [ForeignKey("TournamentId")]
        public int TournamentId { get; set; }

        public Tournament? Tournament { get; set; }
    }
}
