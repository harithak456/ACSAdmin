using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Entity
{
    public class Ent_Organization
    {
        public int Organization_ID { get; set; }
        public string Organization_Name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Organization_Address { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Organization_State { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Organization_Country { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Organization_ContactPerson { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Organization_Phone { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string organization_Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string organization_Web { get; set; }

        public int Is_Active { get; set; }
        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
    }
}