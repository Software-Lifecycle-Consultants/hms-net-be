using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models.Admin;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using HMS.Utilities;

namespace HMS.DTOs.Admin
{
    public class AdminRoomDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The Title must be between 5 and 50 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Subtitle { get; set; }

        [StringLength(100, MinimumLength = 20)]
        public string? DescriptionTitle { get; set; }

        [StringLength(600, MinimumLength = 100)]
        public string? Description { get; set; }

        [RegularExpression(@"^\d+(\.\d{0,2})?$", ErrorMessage = "Price must be a non-negative value maximum of two decimal points.")]
        public decimal Price { get; set; }

        [JsonConverter(typeof(DictionaryStringIntJsonConverter))]
        public Dictionary<int, int> CategoryValuesDictionary { get; set; } = new Dictionary<int, int>();     

        public List<AdminServiceAddonDTO>? ServiceAddons { get; set; }

        public string? AditionalInfoTitle { get; set; }

        public string? AditionalInfoDescription { get; set; }

        // public IFormFile? CoverImage { get; set; }

    }

    public class AdminRoomReturnDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string? Subtitle { get; set; }
       
        public string? DescriptionTitle { get; set; }
        
        public string? Description { get; set; }

        public decimal Price { get; set; } = decimal.MinValue;

        public List<CategoryValueDTO>? AdminCategoryValues { get; set; } 

        public List<AdminServiceAddonDTO>? ServiceAddons { get; set; }

        public string? AditionalInfoTitle { get; set; }

        public string? AditionalInfoDescription { get; set; }

        public string? CoverImagePath { get; set; }
    }


}
