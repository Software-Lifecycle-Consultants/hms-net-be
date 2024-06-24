﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HMS.Models.Admin;

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

        //public List<AdminCategoryDTO> Categories { get; set; } = new List<AdminCategoryDTO>();
        public List<AdminCategoryValueDTO> AdminCategoryValues { get; set; } = new List<AdminCategoryValueDTO>();

        public List<AdminServiceAddonDTO>? ServiceAddons { get; set; }

        public List<AdminAdditionalInfoDTO>? AdditionalInfo { get; set; }

        public string? CoverImagePath { get; set; }
    }
}
