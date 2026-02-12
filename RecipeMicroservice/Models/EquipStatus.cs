using Org.BouncyCastle.Asn1.Mozilla;

namespace RecipeMicroservice.Models
{
    public class Equip
    {
        public int equip_id { get; set; } 
        public string brand { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public string created_by { get; set; } = string.Empty;
        public DateTime created_date { get; set; }
        public string updated_by { get; set; } = string.Empty;
        public DateTime updated_date { get; set; }
    }
    public class EquipStatus
    {
        public int equip_id { get; set; }
        public string brand { get; set; } = string.Empty;
        public string model { get; set; } = string.Empty;
        public int recipe_id { get; set; }
        public string lot_id { get; set; } = string.Empty;
        public int wafer_size { get; set; }
        public int cutting_dept { get; set; }
        public int line_cut { get; set; }
        public string stage { get; set; } = string.Empty;
        public string downloaded_by { get; set; } = string.Empty;
        public DateTime downloaded_date { get; set; }
    }

    public class FormUpdateStatusEquip
    {
        public int EquipID { get; set; }
        public int RecipeID { get; set; }
        public string Stage { get; set; } = string.Empty;
        public string Downloaded_by { get; set; } = string.Empty;
    }

    public class LogEquip
    {
        public int EquipID { get; set; }
        public int RecipeID { get; set; }
        public string Detail { get; set; } = string.Empty;
        public DateTime time_stamp { get; set; }
        public string user_by { get; set; } = string.Empty;
    }

    public class FormLogEquip
    {
        public int EquipID { get; set; }
        public int RecipeID { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string user_by { get; set; } = string.Empty;
    }
}
