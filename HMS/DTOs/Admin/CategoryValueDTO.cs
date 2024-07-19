using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class CategoryValueDTO
    {
        public Guid CatergoryValueID { get; set; }
        public int AdminCategoryId { get; set; }
        public int AdminCategoryValue { get; set; }      
               
    }
}
