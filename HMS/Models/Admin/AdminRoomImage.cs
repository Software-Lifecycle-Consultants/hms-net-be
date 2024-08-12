using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models;
using System.ComponentModel;

namespace HMS.Models.Admin
{
    public class AdminRoomImage : Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public bool IsCoverImage { get; set; } 

        [ForeignKey("AdminRoom")]
        public Guid AdminRoomId { get; set; }
        public AdminRoom AdminRoom { get; set; } = null!;

       
    }


}
