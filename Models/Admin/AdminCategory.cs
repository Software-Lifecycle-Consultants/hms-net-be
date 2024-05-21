using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminCategory
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
       
        public List<string> Values { get; set; } = new List<string>();

        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        public AdminRoom AdminRoom { get; set; } = new AdminRoom();
    }
}
