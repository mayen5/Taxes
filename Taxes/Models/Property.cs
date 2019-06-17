namespace Taxes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int TaxpayerId { get; set; }

        [StringLength(20,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 8)]
        [DataType(DataType.PhoneNumber)]
        public String Phone { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(100,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 10)]
        public String Address { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 7, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public int Stratum { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [Range(1, 999999, ErrorMessage = "The field {0} must be contain values between {1} and {2}")]
        public float Area { get; set; }

        public virtual Taxpayer Taxpayer { get; set; }

        public virtual Department Department { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual PropertyType PropertyType { get; set; }
    }
}