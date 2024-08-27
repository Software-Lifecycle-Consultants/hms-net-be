using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HMS.Models.Admin;

namespace HMS.DTOs.Admin
{
    public class AdminMealsAndServicesValuesDTO
    {
        public string Value { get; set; } = string.Empty;
    }
}
