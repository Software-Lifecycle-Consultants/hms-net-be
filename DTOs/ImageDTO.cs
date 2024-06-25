using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMS.DTOs

{
    public class ImageDTO
    {
        public string? Name { get; set; }

        public IFormFile File { get; set; }
        public string? FilePath { get; set; }
    }
}
