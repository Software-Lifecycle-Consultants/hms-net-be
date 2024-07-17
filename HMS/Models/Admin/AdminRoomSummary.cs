using HMS.DTOs.Admin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminRoomSummary
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; } = decimal.MinValue;

        public List<CategoryValue> AdminCategoryValues { get; set; } = new List<CategoryValue>();

        public string? CoverImagePath { get; set; }
    }
}
