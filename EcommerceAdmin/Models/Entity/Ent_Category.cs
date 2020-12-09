using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceAdmin.Models.Entity
{
    public class Ent_Category
    {
        public int Category_ID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Category_Name { get; set; }

        public int Parent_Category { get; set; }
        public int SubCategoryCount { get; set; }
        public List<Ent_SubCategory> SubCategoryList { get; set; }

        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
    }

    public class Ent_SubCategory
    {
        public int Category_ID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Category_Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public int Parent_Category{ get; set; }
        public int ProductsCount { get; set; }

    }

    public class Ent_Brand
    {
        public int Brand_ID { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Brand_Name { get; set; }

        public int Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public int Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }

    }
}