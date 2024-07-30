using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminGenaralCatagory
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public List<AdminCategory> AdminCategories { set; get; } = null!;
        public List<AdminCategoryValue> AdminCategoriesValues { set; get; } = null!;
    }
}
    