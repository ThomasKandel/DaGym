using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaGym.Models
{
    public class GymClassView
    {
        public int Id { get; set; }
        [Display(Name = "Namn")]
        public string Name { get; set; }
        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Längd")]
        public TimeSpan Duration { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "Deltagare")]
        public ICollection<ApplicationUser> AttendingMembers { get; set; }

        public bool Booked { get; set; }
    }
}


