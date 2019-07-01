namespace Taxes.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class MunicipalitiesView
    {
        public string Department { get; set; }

        public string Municipality { get; set; }

        public List<Municipality> Municipalities { get; set; }

    }
}