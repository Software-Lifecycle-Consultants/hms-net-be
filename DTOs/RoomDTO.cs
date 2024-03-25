using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs
{
    public class RoomDTO
    {
        [MaxLength(50)]
        public string Title { get; set; } = String.Empty;

        public string? Description { get; set; }
        public int GuestCount { get; set; }

        [Required]
        [Range(1, 3)]
        public int BedCount { get; set; }

        [Required]
        [Range(1, 3)]
        public int BathroomCount { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue)]
        public decimal BaseRate { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
