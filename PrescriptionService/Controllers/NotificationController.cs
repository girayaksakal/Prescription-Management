using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly NotificationProcessor _notificationProcessor;

    public NotificationController()
    {
        // Instantiate the NotificationProcessor
        _notificationProcessor = new NotificationProcessor();
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> TriggerLogicApp()
    {
        try
        {
            // Process notifications
            await _notificationProcessor.ProcessNotificationsAsync();

            // Return success response
            return Ok(new
            {
                Message = "Notification Processor triggered successfully.",
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            // Return error response
            return StatusCode(500, new
            {
                Message = "An error occurred while triggering the Notification Processor.",
                Error = ex.Message
            });
        }
    }
}