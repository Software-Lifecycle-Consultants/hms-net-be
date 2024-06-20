using System.ComponentModel.DataAnnotations.Schema;
namespace HMS.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile? file { get; set; }

    }
}
