using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HMS.Models.Admin
{
    public class AdminCategory
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        [ForeignKey("AdminGenaralCatagoty")]
        public int AdminGenaralCatagotyId { get; set; }
        public AdminGenaralCatagory AdminGenaralCatagoty { get; set; } = null!;
        public List<AdminCategoryValue>? AdminCategoryValues { set; get; } 
        
         
    }
   
}

