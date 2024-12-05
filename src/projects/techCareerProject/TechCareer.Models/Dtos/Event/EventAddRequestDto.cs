using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Models.Dtos.Event
{
    public class EventAddRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public string ParticipationText { get; set; }
        public int CategoryId { get; set; }
    }
}
