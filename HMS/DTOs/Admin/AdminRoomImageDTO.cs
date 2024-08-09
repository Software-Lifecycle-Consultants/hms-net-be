using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.DTOs.Admin
{
    public class AdminRoomCoverImageDTO
    {
       public Guid RoomId { get; set; }

        [NotMapped, Required]
        public IFormFile? File { get; set; }
    }

    public class AdminRoomGalleryImagesDTO
    {
        public Guid RoomId { get; set; }

        [NotMapped, Required]
        public List<IFormFile>? Files { get; set; } 
    }
    
    public class AdminRoomImageReturnDTO 
    {
        public Guid RoomId { get; set; }

        public bool IsCoverImage { get; set; }

        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? FilePath { get; set; } 


    }
}
