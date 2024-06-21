using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HMS.DTOs.Admin
{
    public class AdminCategoryValueDTO
    {
        [JsonIgnore]
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Value { get; set; } = string.Empty;

        [JsonIgnore]
        [ForeignKey("AdminCategory")]
        public Guid? AdminCategoryId { get; set; }

        [JsonIgnore]
        public AdminCategory AdminCategory { get; set; } = new AdminCategory();

        [JsonIgnore] 
        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        [JsonIgnore] // Prevent circular reference in Swagger documentation
        public AdminRoom AdminRoom { get; set; } = new AdminRoom();
    }
}
