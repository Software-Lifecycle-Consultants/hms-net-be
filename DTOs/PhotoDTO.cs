using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMS.DTOs
    
{
    public class PhotoDTO
    {
        public string? Name { get; set; }
        public IFormFile? File { get; set; }
        public string? FilePath { get; set; }
    }
}
