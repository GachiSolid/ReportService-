using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Models
{
    public class Report
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
