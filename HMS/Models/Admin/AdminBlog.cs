using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminBlog
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Subtitle { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string BlogContent { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? CoverImage { get; set; }

        [NotMapped]
        public IFormFile? AuthorImage { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public string? AuthorDescription { get; set; }

        public string? Facebook { get; set; }

        public string? Twitter { get; set; }

        public string? LinkedIn { get; set; }

        public DateTime PublishedTime { get; set; } = DateTime.Now;

        public string CoverImagePath { get; internal set; } = string.Empty;

        public string AuthorImagePath { get; internal set; } = string.Empty;
    }
}