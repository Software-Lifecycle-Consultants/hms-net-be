using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminMealsAndServices
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public List<AdminMealsAndServicesValue>? AdminMealsAndServicesValue { get; set; }

    }
}
