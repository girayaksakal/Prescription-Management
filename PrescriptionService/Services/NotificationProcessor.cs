using Azure.Storage.Queues;
using System.Text.Json;

public class NotificationProcessor {
    private readonly string _queueConnectionString = "DefaultEndpointsProtocol=https;AccountName=prescriptionmanagement;AccountKey=oCK2y6gJkUmC/JYQomhXj2podMqFkYf6V+yK5rMN304Q/nvaPWU2VJ0nZ/e2BT5cNWCdE0BNo7B5+ASt44mmAA==;EndpointSuffix=core.windows.net";
    private readonly string _queueName = "prescription-notifications";
    private readonly QueueClient _queueClient;

    public NotificationProcessor() {
        _queueClient = new QueueClient(_queueConnectionString, _queueName);
    }

    public async Task ProcessNotificationsAsync() {
        if(await _queueClient.ExistsAsync()) {
            var response = await _queueClient.ReceiveMessagesAsync(10);
            foreach (var message in response.Value) {
                var prescriptionInfo = JsonSerializer.Deserialize<PrescriptionMessage>(message.MessageText);
                if (prescriptionInfo != null) {
                    await SendEmailAsync(prescriptionInfo.PharmacyEmail, prescriptionInfo.PrescriptionId);
                }

                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
            }
        }
    }
    
    private async Task SendEmailAsync(string pharmacyEmail, string prescriptionId) {
        // Send email to pharmacy
        Console.WriteLine($"Email sent to {pharmacyEmail} for prescription {prescriptionId}");
    }

    private record PrescriptionMessage(string PrescriptionId, string PharmacyEmail);
}