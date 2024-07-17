using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models;
using HMS.Models.Admin;

namespace HMS.DTOs.Admin
{
    public class AdminRoomImage :Image
    {
        public Guid RoomId { get; set; }
        
        public bool IsCoverImage { get; set; }    

    }

   
}
