using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Entities
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
