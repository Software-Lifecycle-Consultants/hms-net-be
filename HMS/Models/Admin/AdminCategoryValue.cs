using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.Models.Admin
{
    public class AdminCategoryValue
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Value { get; set; } = string.Empty;

        [ForeignKey("AdminCategory")]
        public int AdminCategoryId { get; set; }

        public AdminCategory AdminCategory { get; set; } = null!; //null forgiving operator.This tells the compiler that you are aware AdminCategory is not initialized in the constructor and you ensure it will be set late

        [ForeignKey("AdminGenaralCatagoty")]
        public int AdminGenaralCatagotyId { get; set; }
        public AdminGenaralCatagory AdminGenaralCatagoty { get; set; } = null!;
    }
}
