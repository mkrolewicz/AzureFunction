using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionApp2
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([QueueTrigger("idToDownload", Connection = "idQueueStorage")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
