namespace HealthChatbotAPI.Model
{
    public class Alert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? AlertMessage { get; set; }
        public DateTime AlertTime { get; set; }
    }
}
