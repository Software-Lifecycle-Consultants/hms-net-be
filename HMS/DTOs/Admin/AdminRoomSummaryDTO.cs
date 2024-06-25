using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HMS.DTOs.Admin
{
    public class AdminRoomSummaryDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; } = decimal.MinValue;

        public List<AdminCategoryValue> AdminCategoryValues { get; set; } = new List<AdminCategoryValue>();

        public string? CoverImagePath { get; set; }

    }
}
