using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Taxes.Models
{
    [NotMapped]
    public class DepartmentView : Department
    {
        public List<Municipality> MunicipalityList { get; set; }
    }
}