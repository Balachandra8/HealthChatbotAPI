namespace HealthChatbotAPI.Model
{
    public class Symptom
    {
        public int Id { get; set; }
        public string? SymptomDescription { get; set; }
        public DateTime RecordedAt { get; set; }
        public string? Severity { get; set; }
    }
}
