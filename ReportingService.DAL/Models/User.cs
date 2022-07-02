using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Models
{
    public class User: IdentityUser
    {
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }

        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
