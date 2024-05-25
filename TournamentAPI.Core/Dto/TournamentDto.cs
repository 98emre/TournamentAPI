using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentAPI.Core.Dto
{
    public class TournamentDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100")]
        public string Title { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate => StartDate.AddMonths(3);
    }
}
