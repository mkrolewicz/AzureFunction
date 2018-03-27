using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace FunctionApp2
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Queue("idToDownload", Connection = "idQueueStorage")] CloudQueue idToDownload,
            TraceWriter log)
        {
            log.Info($"C# Timer trigger function executed at: {DateTime.Now}");

            var httpClient = new HttpClient();
            
            httpClient.DefaultRequestHeaders.Add("apikey", "c126cc8803b14df2a61bfc37e4b299f6");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var result = httpClient.GetStringAsync(new Uri("https://airapi.airly.eu/v1/sensors/current?southwestLat=50&southwestLong=19.9&northeastLat=50.09&northeastLong=20.02"));
            dynamic data = JsonConvert.DeserializeObject(result.Result);

            foreach (var t in data)
            {
                var cloudQueueMessage = new CloudQueueMessage(t.ToString());
                idToDownload.AddMessageAsync(cloudQueueMessage);
            }
        }
    }
}
