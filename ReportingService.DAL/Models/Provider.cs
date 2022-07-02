using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Models
{
    public class Provider
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }

        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
