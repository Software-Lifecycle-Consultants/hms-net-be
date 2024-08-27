using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminMealsAndServicesValue
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string Value { get; set; } = string.Empty;

        [ForeignKey("AdminMealsAndServicesValues")]
        public int AdminMealsAndServicesId { get; set; }

        public AdminMealsAndServices AdminMealsAndServices { get; set; } = null!;
    }
}
