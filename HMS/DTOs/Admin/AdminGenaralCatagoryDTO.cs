using HMS.Models.Admin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;

namespace HMS.DTOs.Admin
{
    public class AdminGenaralCatagoryDTO
    {
        
        public List<AdminCategoryGenaralDTO> AdminCategories { set; get; } = null!;
        public List<AdminFAQDTOs> AdminFAQs { set; get; } = null!;


    }
    public class AdminFAQDTOs
    { 
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        
    }
    public class AdminCategoryGenaralDTO 
    {
        //[NonZeroId]
        //public int Id { get; set; }     

        public string Title { get; set; } = string.Empty;
   
        public List<AdminCategoryValueGenaralDTO>? AdminCategoryValues { set; get; }


    }
    public class AdminCategoryValueGenaralDTO
    {
       

        public string Value { get; set; } = string.Empty;



    }


public class NonZeroIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int intValue && intValue == 0)
            {
                return new ValidationResult("Admin Catagory Id cannot be 0");
            }
            else 
            { 
                return ValidationResult.Success; 
            }
            
        }
    }

    //public class ReturnAdminGenaralCatagoryDTO
    //{
    //    public Guid Id { get; set; }
    //    public List<ReturnAdminCategoryValue> AdminCategories { set; get; } = null!;


    //}
    //public class ReturnAdminCategoryValue
    //{
    //    public Guid Id { get; set; }

    //    public string Title { get; set; } = string.Empty;
    //    public List<ReturnAdminCategory>? ReturnAdminCategoryValues { set; get; }
    //}
    //public class ReturnAdminCategory
    //{

    //    public string Value { get; set; } = string.Empty;


    //    public AdminCategory? AdminCategory { get; set; }

    //}
}
