using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Text.Json.Serialization;
namespace HMS.Models
{
public class Image
{
    
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string? Name { get; set; }

    [NotMapped]
    public List<IFormFile>? Files { get; set; } // Do we need this property here?
        
    public string FilePath { get; set; } = String.Empty;

}}
