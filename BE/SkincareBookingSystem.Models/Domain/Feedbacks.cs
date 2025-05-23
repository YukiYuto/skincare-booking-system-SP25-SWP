﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class Feedbacks : BaseEntity<string, string, string>
    {
        [Key]
        public Guid FeedbackId { get; set; }
        [StringLength(30)] public string Title { get; set; } = null!;
        [StringLength(100)] public string Content { get; set; } = null!;
        [Range(1, 5)]public int Rating { get; set; }

        public Guid AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual Appointments Appointments { get; set; } = null!;
    }
}
