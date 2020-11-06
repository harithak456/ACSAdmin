using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Entity
{
    public class Ent_Order
    {
        public int Order_ID { get; set; }
        public int Guest_ID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_FirstName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_LastName { get; set; }

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

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Phone { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Guest_Email { get; set; }  
        public double Order_SubTotal { get; set; }  
        public double Order_Shipping { get; set; }  
        public double Order_Total { get; set; }  
        public int Total_Qty { get; set; }  

        public int Is_Active { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Modified_Date { get; set; }
        public List<Ent_OrderDetail> OrderDetailsList = new List<Ent_OrderDetail>();
        public Ent_Guest entGuest = new Ent_Guest();
    }
    public class Ent_OrderDetail
    {
        public int OrderDetail_ID { get; set; }
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
        public float Product_Price { get; set; }
        public float Product_Total { get; set; }
        public string Product_Name { get; set; }
        public string Product_Image { get; set; }

        public int Cart_ID { get; set; }
        public int Guest_ID { get; set; }
        public Ent_Order entOrder = new Ent_Order();
    }
    }