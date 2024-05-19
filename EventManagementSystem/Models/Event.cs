﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace EventManagementSystem.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [ForeignKey("User")]
        public int UserId{ get; set; }
        [Required]
        public User User { get; set; } = default!;

        public DateTime Create_at { get; set; } = DateTime.Now;
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(100)]
        public string VenueName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Directions { get; set; } = string.Empty;
        public List<Transport>? Transports { get; set; }

        public ICollection<Category>? Categories { get; set; }
        public ICollection<TicketType>? TicketTypes { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}
