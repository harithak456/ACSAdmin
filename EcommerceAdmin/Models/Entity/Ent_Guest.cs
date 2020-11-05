using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Entity
{
    public class Ent_Guest
    {
        public int Guest_ID { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_FirstName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Username { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Password { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Unique_ID { get; set; }

        public int Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
    }
}