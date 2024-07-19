using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Room
    {
        [ Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = String.Empty;

        public  string? Description { get; set; }
        public int GuestCount { get; set; }

        [Required]
        [Range(1,3)]
        public int BedCount { get; set; }

        [Required]
        [Range(1, 3)]
        public int BathroomCount { get; set; }

        [DataType (DataType.Currency)]
        [Range(0.01,double.MaxValue)]
        public decimal BaseRate { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? CoverImagePath { get; set; }
    }
}
