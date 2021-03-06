﻿namespace Taxes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(30,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 2)]
        [Display(Name = "First name")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(30,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 2)]
        [Display(Name = "Last name")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(50,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 7)]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Index("Employee_UserName_Index", IsUnique = true)]
        public String UserName { get; set; }

        [StringLength(20,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 8)]
        [DataType(DataType.PhoneNumber)]
        public String Phone { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int MunicipalityId { get; set; }

        [StringLength(80,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 8)]
        public String Address { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        public int DocumentTypeId { get; set; }

        [Required(ErrorMessage = "You must enter a {0}")]
        [StringLength(20,
            ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 13)]
        [Index("Taxpayer_Document_Index", IsUnique = true)]
        public String Document { get; set; }

        public int? BossId { get; set; }

        [Display(Name = "Full name")]
        public String FullName { get {return String.Format("{0} {1}", this.FirstName, this.LastName) ; } }

        public virtual Department Department { get; set; }

        public virtual Municipality Municipality { get; set; }

        public virtual DocumentType DocumentType { get; set; }

        public virtual Employee Boss { get; set; }

        public virtual List<Employee> Employees { get; set; }

    }
}