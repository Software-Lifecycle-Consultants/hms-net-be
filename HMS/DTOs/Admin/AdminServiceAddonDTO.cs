using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminServiceAddonDTO
    {
        [StringLength(30, MinimumLength = 10)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300, MinimumLength = 100)]
        public string? Description { get; set; }

        public List<string>? Adons { get; set; }
    }
}
