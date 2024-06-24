namespace HMS.DTOs.Admin
{
    public class AdminAdditionalInfoDTO
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public List<string>? Adons { get; set; }
    }
}
