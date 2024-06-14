using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminBlog
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The Title must be between 10 and 100 characters.")]
        public string Title { get; set; } = string.Empty;


        [StringLength(150, MinimumLength = 10)]
        public string? Subtitle { get; set; }

        [NotMapped]
        public IFormFile? CoverImage { get; set; }

        [Required]
        public DateTime PublishedTime { get; set; }


        public List<string> Tags { get; set; } = [];

        [Required]
        [StringLength(10000, MinimumLength = 100)]
        public string BlogContent { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string AuthorName { get; set; } = string.Empty;


        [StringLength(500, MinimumLength = 10)]
        public string? AuthorDescription { get; set; }

        [NotMapped]
        public IFormFile? AuthorImage { get; set; }


        [Url]
        public string? Facebook { get; set; }


        [Url]
        public string? Twitter { get; set; }


        [Url]
        public string? LinkedIn { get; set; }
    }
}
