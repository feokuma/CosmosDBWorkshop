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
                Uri collectionLink = UriFactory.CreateDocumentCollectionUri("EntertainmentDatabase", "CustomCollection");
                var foodInteractions = new Bogus.Faker<PurchaseFoodOrBeverage>()
                    .RuleFor(i => i.type, (fake) => nameof(PurchaseFoodOrBeverage))
                    .RuleFor(i => i.unitPrice, (fake) => Math.Round(fake.Random.Decimal(1.99m, 15.99m), 2))
                    .RuleFor(i => i.quantity, (fake) => fake.Random.Number(1, 5))
                    .RuleFor(i => i.totalPrice, (fake, user) => Math.Round(user.unitPrice * user.quantity, 2))
                    .Generate(500);
                foreach (var interaction in foodInteractions)
                {
                    ResourceResponse<Document> result = await client.CreateDocumentAsync(collectionLink, interaction);
                    await Console.Out.WriteLineAsync($"Document #{foodInteractions.IndexOf(interaction):000} Created\t{result.Resource.Id}");
                }
            }
        }
    }
}
