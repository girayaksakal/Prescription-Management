using Azure.Storage.Queues;
using System.Text.Json;

public class NotificationQueueService {
    private readonly string _queueConnectionString = "DefaultEndpointsProtocol=https;AccountName=prescriptionmanagement;AccountKey=oCK2y6gJkUmC/JYQomhXj2podMqFkYf6V+yK5rMN304Q/nvaPWU2VJ0nZ/e2BT5cNWCdE0BNo7B5+ASt44mmAA==;EndpointSuffix=core.windows.net";
    private readonly string _queueName = "prescription-notifications";
    private readonly QueueClient _queueClient;

    public NotificationQueueService() {
        _queueClient = new QueueClient(_queueConnectionString, _queueName);
        _queueClient.CreateIfNotExists();
    }

    public async Task AddPrescriptionToQueueAsync(string prescriptionId, string pharmacyEmail) {
        var message = JsonSerializer.Serialize(new {
            PrescriptionId = prescriptionId,
            PharmacyEmail = pharmacyEmail
        });
        
        await _queueClient.SendMessageAsync(message);
    }
}