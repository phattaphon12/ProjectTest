namespace RecipeMicroservice.Models
{
    public class Form
    {
        
    }

    public class FormGetRecipeByLotID
    {
        public string lot_id { get; set; } = string.Empty;
    }

    public class FormInsertRecipe
    {
        public string lot_id { get; set; } = string.Empty;
        public int wafer_size { get; set; }
        public int cutting_dept { get; set; }
        public int line_cut { get; set; }
        public string created_by { get; set; } = string.Empty;
        public string updated_by { get; set; } = string.Empty;
    }

    public class FormUpdateRecipe
    {
        public string lot_id { get; set; } = string.Empty;
        public int wafer_size { get; set; }
        public int cutting_dept { get; set; }
        public int line_cut { get; set; }
        public string updated_by { get; set; } = string.Empty;
    }

    public class UnflagRecipe
    {
        public string lot_id { get; set; } = string.Empty;
        public string updated_by { get; set; } = string.Empty;
    }
}
