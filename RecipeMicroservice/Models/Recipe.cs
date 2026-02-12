namespace RecipeMicroservice.Models
{
    public class Recipe
    {
        public int recipe_id { get; set; }
        public string lot_id { get; set; } = string.Empty;
        public int wafer_size { get; set; }
        public int cutting_dept { get; set; }
        public int line_cut { get; set; }
        public string created_by { get; set; } = string.Empty;
        public DateTime created_date { get; set; }
        public string updated_by { get; set; } = string.Empty;
        public DateTime update_date { get; set; }
        public bool flag { get; set; }
    }

    public class RecipeFromPLC
    {
        public int DeptZ1 { get; set; }
        public int DeptZ2 { get; set; }
        public int WaferSizeZ1 { get; set; }
        public int WaferSizeZ2 { get; set; }
        public int LineZ1 { get; set; }
        public int LineZ2 { get; set; }
    }
}
