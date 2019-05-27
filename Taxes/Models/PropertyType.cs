using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxes.Models
{
    public class PropertyType
    {
        [Key]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public String Description { get; set; }

        [DataType(DataType.MultilineText)]
        public String Notes { get; set; }

    }
}