using System.ComponentModel.DataAnnotations;

namespace ReportingService.DAL.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int ProviderId { get; set; }
        public int Sum { get; set; }
        [Required]
        public int ReportId { get; set; }

        public virtual Report Report { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
