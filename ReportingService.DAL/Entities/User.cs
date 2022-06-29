using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
