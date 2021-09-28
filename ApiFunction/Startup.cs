using Azure.Identity;
using ApiFunctions;
using ApiFunctions.Configurations;
using ApiFunctions.Data;
using ApiFunctions.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using System;
using System.Reflection;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Options;
using ApiFunctions.Models;

[assembly: FunctionsStartup(typeof(GroupPaintOnlineWebApp.ApiFunction.Startup))]
namespace GroupPaintOnlineWebApp.ApiFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<CosmosDbConfiguration>()
               .Configure<IConfiguration>((settings, configuration) =>
               {
                   settings.AccountEndpoint=configuration["CosmosAccountEndpoint"];
                   settings.AccountKey = configuration["CosmosAccountKey"];
                   settings.DatabaseName = Environment.GetEnvironmentVariable("DatabaseName");
                   settings.RoomContainerName = Environment.GetEnvironmentVariable("RoomContainerName");
                   settings.RoomContainerPartitionKeyPath = Environment.GetEnvironmentVariable("RoomContainerPartitionKeyPath");
                   settings.PaintingContainerName = Environment.GetEnvironmentVariable("PaintingContainerName");
                   settings.PaintingContainerPartitionKeyPath = Environment.GetEnvironmentVariable("PaintingContainerPartitionKeyPath");
                   builder.Services.AddSingleton(settings);
               });


            builder.Services.TryAddSingleton(implementationFactory =>
            {
                var cosmosDbConfiguration = implementationFactory.GetRequiredService<IOptions<CosmosDbConfiguration>>();
                CosmosClient cosmosClient = new CosmosClient(cosmosDbConfiguration.Value.AccountEndpoint, cosmosDbConfiguration.Value.AccountKey);
                Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDbConfiguration.Value.DatabaseName).GetAwaiter().GetResult();
                database.CreateContainerIfNotExistsAsync(cosmosDbConfiguration.Value.RoomContainerName,
                    cosmosDbConfiguration.Value.RoomContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();

                database.CreateContainerIfNotExistsAsync(cosmosDbConfiguration.Value.PaintingContainerName,
                    cosmosDbConfiguration.Value.PaintingContainerPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();

                return cosmosClient;
            });

            builder.Services.AddSingleton<ICosmosDbService<Room>, RoomRepository>();
            builder.Services.AddSingleton<ICosmosDbService<Painting>, PaintingRepository>();

        }
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var builtConfig = builder.ConfigurationBuilder.Build();
            

            var keyVaultEndpoint = Environment.GetEnvironmentVariable("VaultUri");
            if (!string.IsNullOrEmpty(keyVaultEndpoint))
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.ConfigurationBuilder
                   .AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            }
                builder.ConfigurationBuilder
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("local.settings.json", true)
                   .AddEnvironmentVariables()
                   .Build();
            
        }
    }
}
