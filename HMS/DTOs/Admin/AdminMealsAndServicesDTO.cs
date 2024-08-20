using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminMealsAndServicesDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public List<AdminMealsAndServicesValuesDTO>? AdminMealsAndServicesValues { get; set; }
    }
}
