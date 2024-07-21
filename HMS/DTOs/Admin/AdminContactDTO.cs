using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminContactDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "The Title must be between 5 and 20 characters.")]
        public string PageTitle { get; set; } = string.Empty;

        [StringLength(200, MinimumLength = 50)]
        public string PageDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "The Email is not a valid e-mail address.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(15, MinimumLength= 7)]
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
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The Zip Code must be between 4 and 10 characters.")]
        public string ZipCode { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

    }
}
