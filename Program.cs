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
                await client.OpenAsync();
                Uri databaseLink = UriFactory.CreateDatabaseUri("EntertainmentDatabase");
                IndexingPolicy indexingPolicy = new IndexingPolicy
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true,
                    IncludedPaths = new Collection<IncludedPath>
                    {
                        new IncludedPath
                        {
                            Path = "/*",
                            Indexes = new Collection<Index>
                            {
                                new RangeIndex(DataType.Number, -1),
                                new RangeIndex(DataType.String, -1)
                            }
                        }
                    }
                };
                
                PartitionKeyDefinition partitionKeyDefinition = new PartitionKeyDefinition
                {
                    Paths = new Collection<string> { "/type" }
                };
                DocumentCollection customCollection = new DocumentCollection
                {
                    Id = "CustomCollection",
                    PartitionKey = partitionKeyDefinition,
                    IndexingPolicy = indexingPolicy
                };
                RequestOptions requestOptions = new RequestOptions
                {
                    OfferThroughput = 10000
                };
                customCollection = await client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, customCollection, requestOptions);
                await Console.Out.WriteLineAsync($"Custom Collection Self-Link:\t{customCollection.SelfLink}");
            }
        }
    }
}
