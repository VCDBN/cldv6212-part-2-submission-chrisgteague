
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=sacteague;AccountKey=wBk08kAtZ9XqwzzJSd5uHCZgfyRL45n2udQ1moSfRYueH5eGFTl+UpZtDG0itnuUongHpIirICWB+AStM+cKJQ==;EndpointSuffix=core.windows.net";

string queueName = "st10083450-queue";

QueueClient queueClient = new QueueClient(connectionString, queueName);

Console.WriteLine($"Creating Queue {queueName}");
await queueClient.CreateIfNotExistsAsync();



Console.WriteLine("\nAdding messages to the queue");

string[] VacInfo =
{"1206057899182:VaccinationCenterA:2023-05-13:CD456Y" , "3305078029088:VaccinationCenterB:2023-07-20:GH012W", "6706057899197:VaccinationCenterC:2023-09-11:KL678U" , "2306057895156:VaccinationCenterD:2023-10-04:EF789Z" , "8905153998184:VaccinationCenterE:2023-12-25:IJ345V"

};

foreach (string v in VacInfo)
{
    string encodedStr1 = Convert.ToBase64String(Encoding.UTF8.GetBytes(v));
    await queueClient.SendMessageAsync(encodedStr1);
}



Console.WriteLine($"\nPeeking at messages in {queueName}");
PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages: 10);

foreach (PeekedMessage message in peekedMessages)
{
    Console.WriteLine($"Message: {message.MessageId}  :  {message.Body}   :   {message.DequeueCount}   :   {message.ExpiresOn}");
}

Console.WriteLine("\nExecution Completed");
