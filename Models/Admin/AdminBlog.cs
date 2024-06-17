using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminBlog
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Title must be between 3 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 5, ErrorMessage = "The Sub Title must be between 5 and 150 characters.")]
        public string? Subtitle { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? CoverImage { get; set; }

        [Required]
        public DateTime PublishedTime { get; set; }

        [Required]
        public List<string> Tags { get; set; } = [];

        [Required]
        [MinLength(10, ErrorMessage = "The content must be more 10 than characters.")]
        public string BlogContent { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Author Name must be between 3 and 50 characters.")]
        public string AuthorName { get; set; } = string.Empty;

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "The Author Name must be between 10 and 500 characters.")]
        public string? AuthorDescription { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? AuthorImage { get; set; }

        [Required]
        [Url]
        public string? Facebook { get; set; }

        [Required]
        [Url]
        public string? Twitter { get; set; }

        [Required]
        [Url]
        public string? LinkedIn { get; set; }
    }
}
