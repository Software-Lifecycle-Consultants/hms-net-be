using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminFAQDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Question { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Answer { get; set; } = string.Empty;
    }
}