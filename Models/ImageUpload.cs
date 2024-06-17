using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models
{
    public class ImageUpload
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ImageTest { get; set; }
        
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
