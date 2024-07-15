using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HMS.Models.Admin
{
    public class CategoryValue
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("AdminCategoryValues")]
        public int AdminCategoryValuesId { get; set; }

        public AdminCategoryValue AdminCategoryValues { get; set; } = null!;

        [ForeignKey("AdminRoom")]
        public Guid AdminRoomId { get; set; }

        public AdminRoom AdminRoom { get; set; } = null!;

    }
}
