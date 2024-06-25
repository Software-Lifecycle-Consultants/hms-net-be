namespace HMS.DTOs
{
    public class PhotoDTO
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? file { get; set; }
    }
}
