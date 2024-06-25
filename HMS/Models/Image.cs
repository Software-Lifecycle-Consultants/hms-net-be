using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class Image
    {
        [Required,Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FullSizePath { get; set; } = string.Empty;

        public string? DisplaySizePath { get; set; }
        public string? ThumbNailPath { get; set; }

        public byte[] ImageData { get; set; } = new byte[0];

        [NotMapped]
        public IFormFile? ImageFile { get; set; }    
    }
}
