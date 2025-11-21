namespace Authentication.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public DateTime create_date { get; set; }
        public DateTime update_date { get; set; }
    }
}
