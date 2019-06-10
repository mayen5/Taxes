namespace Taxes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Municipality
    {
        [Key]
        public int MunicipalityId { get; set; }

        [Display(Name = "Departament Name")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        //[Index("Municipality_Name_Index", IsUnique = true)]
        [StringLength(255, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters",
            MinimumLength = 1)]
        [Display(Name = "Municipality Name")]
        public String Name { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Taxpayer> Taxpayers { get; set; }

    }
}