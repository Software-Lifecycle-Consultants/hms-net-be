using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;
namespace HMS.Models
{
public class Photo
{
    
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Name { get; set; }

    [NotMapped]
    public IFormFile? File { get; set; }
    public string? FilePath { get; set; }

}}
