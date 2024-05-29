using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HMS.Models.Admin
{
    public class AdminAdditionalInfo
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<string>? Adons { get; set; }

        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        [JsonIgnore] // Prevent circular reference in Swagger documentation
        public AdminRoom AdminRoom { get; set; } = new AdminRoom();
    }
}
