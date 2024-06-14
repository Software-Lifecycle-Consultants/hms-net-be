using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Models.Admin
{
    public class AdminContacts
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The Title must be between 10 and 100 characters.")]
        public string PageTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string PageDescription { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "The Email is not a valid e-mail address.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        [Phone(ErrorMessage = "The Telephone Number is not a valid phone number.")]
        public string phoneNumber { get; set; } = string.Empty;

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;

        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string StateProvince { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "The Zip Code must be between 5 and 10 characters.")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;

    }
}