using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminBlogDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;
        
        public string? Subtitle { get; set; }

        [Required]
        public List<string> Tags { get; set; } = [];

        [Required]
        [MinLength(10, ErrorMessage = "The content must be more 10 than characters.")]
        public string BlogContent { get; set; } = string.Empty;

        public IFormFile? CoverImage { get; set; }

        public IFormFile? AuthorImage { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Author Name must be between 3 and 50 characters.")]
        public string AuthorName { get; set; } = string.Empty;
        
        public string? AuthorDescription { get; set; }
        
        [Url]
        public string? Facebook { get; set; }
        
        [Url]
        public string? Twitter { get; set; }
        
        [Url]
        public string? LinkedIn { get; set; }

    }

    public class AdminBlogReturnDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Subtitle { get; set; }

        public List<string> Tags { get; set; } = [];

        public string BlogContent { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public string? AuthorDescription { get; set; }

        public string? Facebook { get; set; }

        public string? Twitter { get; set; }

        public string? LinkedIn { get; set; }

        public string CoverImagePath { get; set; } = string.Empty;

        public string AuthorImagePath { get; set; } = string.Empty;

        public DateTime PublishedTime { get; set; }

    }
}
