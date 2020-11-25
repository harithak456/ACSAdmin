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
        public string Guest_LastName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Phone { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Username { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Password { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Unique_ID { get; set; }

        public int Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
        public Ent_GuestAddress entGuestAddress = new Ent_GuestAddress();
    }
    public class Ent_GuestAddress
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string First_Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Last_Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Country { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Address1 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Address2 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Town { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_State { get; set; }
        public string Address_Type { get; set; }
        public int Guest_ID { get; set; }
        public int Address_ID { get; set; }

        public int Address_Default { get; set; }

    }
}