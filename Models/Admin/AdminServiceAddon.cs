using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminServiceAddon
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
       
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }
       
        public List<string>? Adons { get; set; }

        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        public AdminRoom AdminRoom { get; set; } = new AdminRoom();
    }
}
