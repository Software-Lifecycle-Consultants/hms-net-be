using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminRoom
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The Title must be between 10 and 100 characters.")]
        public string Title { get; set; }= string.Empty;

        public string? Subtitle { get; set; }

        public string? DescriptionTitle { get; set; }

        public string? Description { get; set; }

        [NotMapped]
        public IFormFile? CoverImage { get; set; }           

        public List<AdminCategory> Categories { get; set; } = new List<AdminCategory>();

        public List<AdminServiceAddon>? ServiceAddons { get; set; }

        public List<AdminAdditionalInfo>? AdditionalInfo { get; set; }
    }
}
