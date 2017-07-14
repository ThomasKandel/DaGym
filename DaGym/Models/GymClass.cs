using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DaGym.Models
{
    public class GymClass
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }
        [Required]
        [Display(Name = "Längd")]
        public TimeSpan Duration { get; set; }
        [Display(Name = "Slut")]
        public DateTime EndTime { get { return StartTime + Duration; } }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> AttendingMembers { get; set; }
    }
}