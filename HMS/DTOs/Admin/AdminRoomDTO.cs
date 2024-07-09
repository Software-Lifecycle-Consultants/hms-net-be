﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models.Admin;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HMS.DTOs.Admin
{
    public class AdminRoomDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Title must be between 5 and 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Subtitle { get; set; }

        [StringLength(100)]
        public string? DescriptionTitle { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public decimal Price { get; set; } = decimal.MinValue;

        public Dictionary<string, string> CategoryValuesDictionary { get; set; } = new Dictionary<string, string>();

        [JsonIgnore]
       // [BindNever]
        public List<AdminCategoryValueDTO> AdminCategoryValues { get; set; } = new List<AdminCategoryValueDTO>();

        public List<AdminServiceAddonDTO>? ServiceAddons { get; set; }

        public List<AdminAdditionalInfoDTO>? AdditionalInfo { get; set; }

       // public IFormFile? CoverImage { get; set; }



    }

    public class AdminRoomReturnDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string? Subtitle { get; set; }
       
        public string? DescriptionTitle { get; set; }
        
        public string? Description { get; set; }

        public decimal Price { get; set; } = decimal.MinValue;

        public List<AdminCategoryReturnValueDTO>? AdminCategoryValues { get; set; } 

        public List<AdminServiceAddonDTO>? ServiceAddons { get; set; }

        public List<AdminAdditionalInfoDTO>? AdditionalInfo { get; set; }

        public string? CoverImagePath { get; set; }
    }


}
