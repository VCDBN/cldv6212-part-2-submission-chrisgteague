using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;


namespace st10083450_QueueTrigger
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("st10083450-queue", Connection = "QueueConnStr")]string myQueueItem, ILogger log)
        {
            string SQLConnStr = Environment.GetEnvironmentVariable("SQLConnectString", EnvironmentVariableTarget.Process);

            try
            {
                string[] information = myQueueItem.Split(':');

                string id = information[0];
                string vacCent = information[1];
                string vacDate = information[2];
                string vacSNum = information[3];

                log.LogInformation($"Processing queue message: {myQueueItem}");

                using (SqlConnection connection = new SqlConnection(SQLConnStr))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand()) 
                    {
                        command.CommandText = "INSERT INTO VacData (id, vacCent, vacDate, vacSNum) VALUES (@Id, @VacCent, @VacDate, @VacSNum)";
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@VacCent", vacCent);
                        command.Parameters.AddWithValue("@VacDate", vacDate);
                        command.Parameters.AddWithValue("@VacSNum", vacSNum);
                        command.ExecuteNonQuery();
                    }
                }

                    log.LogInformation($"Queue message sucessfully stored in the database Id={id}, VacCent={vacCent}, VacDate={vacDate}, VacSNum={vacSNum}");
            }
            catch(Exception ex) 
            {
                log.LogInformation($"Error processing message: {ex.Message}");
            }

        }

    }
}
