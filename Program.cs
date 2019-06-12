using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace cosmosdb_lab
{
    class Program
    {
        private static readonly Uri _endpointUri = new Uri("https://cosmosgroup-lab-feokuma.documents.azure.com:443/");
        private static readonly string _primaryKey = "AuF5Fu0P6p0pDBeRXMBzI9gL1AM9IOV4xHnNSUP26DdUDx3qNPex8s5MMtmvBghyGwfiiZ6GPQgkAJbwC54HQA==";

        public static async Task Main(string[] args)
        {
            using (DocumentClient client = new DocumentClient(_endpointUri, _primaryKey))
            {
                Database targetDatabase = new Database { Id = "EntertainmentDatabase" };
                targetDatabase = await client.CreateDatabaseIfNotExistsAsync(targetDatabase);

                await Console.Out.WriteLineAsync($"Database Self-Link:\t{targetDatabase.SelfLink}");
            }
        }
    }
}
