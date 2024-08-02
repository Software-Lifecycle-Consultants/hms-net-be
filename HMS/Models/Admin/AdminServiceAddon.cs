using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HMS.Models.Admin
{
    public class AdminServiceAddon
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(30, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300, MinimumLength = 100)]
        public string? Description { get; set; }

        public List<string>? Adons { get; set; }

        [ForeignKey("AdminRoom")]
        public Guid? AdminRoomId { get; set; }

        [JsonIgnore] // Prevent circular reference in Swagger documentation
        public AdminRoom AdminRoom { get; set; } = null!;
    }
}
