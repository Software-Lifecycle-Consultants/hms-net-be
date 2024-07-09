using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HMS.Models.Admin
{
    public class AdminCategoryValue
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;

        [ForeignKey("AdminCategory")]
        public Guid? AdminCategoryId { get; set; }

        public AdminCategory? AdminCategory { get; set; } 
        //since the values in category table is added seperately, i dnt need to create new entry here

        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        public AdminRoom AdminRoom { get; set; } = new AdminRoom();

    }
}
