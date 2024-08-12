using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HMS.DTOs.Admin
{
    public class AdminCategoryDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }

    public class AdminCatagoryReturnDTO 
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }
   
}
