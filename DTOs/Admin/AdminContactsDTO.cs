using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminContactsDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The Title must be between 10 and 100 characters.")]
        public string PageTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string PageDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "The Email is not a valid e-mail address.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        [Phone(ErrorMessage = "The Telephone Number is not a valid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; } = string.Empty;

        [StringLength(100)]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string StateProvince { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "The Zip Code must be between 5 and 10 characters.")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

    }
}
