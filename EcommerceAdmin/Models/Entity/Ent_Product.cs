using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Entity
{
    public class Ent_Product
    {
        public int Product_ID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Product_Name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Product_SubText { get; set; }

        public int Category_ID { get; set; }
        public int SubCategory_ID { get; set; }
        public int Brand_ID { get; set; }
        public int Quantity { get; set; }

        public float Product_Price { get; set; }
        public float Product_Discount { get; set; }
        public float Product_Total { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Product_Description { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Product_ImageFile { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Product_Image { get; set; }

        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }

        public Ent_Category entCategory = new Ent_Category();
        public Ent_SubCategory entSubCategory = new Ent_SubCategory();
        public Ent_Brand entBrand= new Ent_Brand();

    }
}