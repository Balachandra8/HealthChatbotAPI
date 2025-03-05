using HealthChatbotAPI.Hubs;
using HealthChatbotAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;


[Route("api/[controller]")]
[ApiController]
public class ChatbotController : ControllerBase
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ApplicationDbContext _dbContext;

    public ChatbotController(IHubContext<ChatHub> hubContext, ApplicationDbContext dbContext)
    {
        _hubContext = hubContext;
        _dbContext = dbContext;
    }

    public class StartChatRequest
    {
        public int UserId { get; set; }
    }

    // Endpoint to start a chatbot session
    [HttpPost("start")]
    public async Task<IActionResult> StartChat([FromBody] StartChatRequest request)
    {
        var session = new ChatbotSession
        {
            UserId = request.UserId,
            StartTime = DateTime.Now,
            IsActive = true
        };

        _dbContext.ChatbotSessions.Add(session);
        await _dbContext.SaveChangesAsync();

        return Ok(new { Message = "Chatbot session started", SessionId = session.Id });
    }

    // Endpoint to send user symptoms and get a response
    [HttpPost("response")]
    public async Task<IActionResult> GetChatbotResponse([FromBody] Symptom symptom)
    {
        // Store the symptom in the database
        _dbContext.Symptoms.Add(symptom);
        await _dbContext.SaveChangesAsync();

        string triageResponse = EvaluateSymptom(symptom);

        await _hubContext.Clients.User(symptom.Id.ToString()).SendAsync("ReceiveMessage", triageResponse);

        return Ok(new { Message = "Response sent to the user" });
    }

    // Endpoint to get chat history for a user
    [HttpGet("history/{userId}")]
    public IActionResult GetChatHistory(int userId)
    {
        var sessionHistory = _dbContext.ChatbotSessions
            .Where(s => s.UserId == userId)
            .ToList();
        return Ok(sessionHistory);
    }

    public class EmergencyAlertRequest
    {
        public int UserId { get; set; }
        public string? AlertMessage { get; set; }
    }


    [HttpPost("emergency")]
    public async Task<IActionResult> SendEmergencyAlert([FromBody] EmergencyAlertRequest alertRequest)
    {
        // Create a new alert in the database
        var alert = new Alert
        {
            UserId = alertRequest.UserId,
            AlertMessage = alertRequest.AlertMessage,
            AlertTime = DateTime.Now
        };

        _dbContext.Alerts.Add(alert);
        await _dbContext.SaveChangesAsync();

        await _hubContext.Clients.User(alertRequest.UserId.ToString()).SendAsync("ReceiveAlert", alert.AlertMessage);

        return Ok(new { Message = "Emergency alert sent", AlertId = alert.Id });
    }

    private string EvaluateSymptom(Symptom symptom)
    {
        // Logic to evaluate the symptom and return a triage message
        if (symptom.Severity == "High")
        {
            return "Immediate attention required! Please seek emergency care.";
        }
        else if (symptom.Severity == "Medium")
        {
            return "You may want to visit a healthcare provider soon.";
        }
        else
        {
            return "Your symptoms seem to be manageable, please monitor your condition.";
        }
    }
}
