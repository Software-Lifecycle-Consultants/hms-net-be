using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMS.DTOs

{
    public class ImageDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public List<IFormFile> Files { get; set; }

        public string? FilePath { get; set; }

    }
}
