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
        [StringLength(100, MinimumLength = 10)]
        public string Title { get; set; }= string.Empty;

        public string? Subtitle { get; set; }

        public string? DescriptionTitle { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; } = decimal.MinValue;

        public List<CategoryValue> CategoryValues { get; set; } = new List<CategoryValue>();

        public List<AdminServiceAddon>? ServiceAddons { get; set; }

        public string? AditionalInfoTitle { get; set; }

        public string? AditionalInfoDescription{ get; set; }

        //public List<AdminAdditionalInfo>? AdditionalInfo { get; set; }

        //public string? CoverImagePath { get; set; }

        public Guid? CoverImageId { get; set;}

        //public Image Image { get; set; }


    }
}
