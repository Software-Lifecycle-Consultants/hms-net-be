using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMS.DTOs

{
    public class ImageDTO
    {       
        public List<IFormFile>? Files { get; set; }    
    }

    public class ImageReturnDTO
    {
        public Guid Id { get; set; }

        //public string? Name { get; set; }

        public string? FilePath { get; set; }

        public bool Success { get; set; }

        public string? ErrorMessage { get; set; }

    }
}
