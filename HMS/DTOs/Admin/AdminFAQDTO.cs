using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("AdminGenaralCatagoty")]
        public Guid AdminGenaralCatagotyId { get; set; }
        public AdminGenaralCatagory AdminGenaralCatagoty { get; set; } = null!;
    }
}