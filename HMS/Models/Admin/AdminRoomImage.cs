using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models;

namespace HMS.Models.Admin
{
    public class AdminRoomImage : Image
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoomId { get; set; }

        [Required]
        public bool IsCoverImage { get; set; }
    }


}
