namespace HMS.DTOs.Admin
{
    public class AdminCategoryDTO
    {
        public string Title { get; set; } = string.Empty;

        public List<string> Values { get; set; } = new List<string>();
    }
}
