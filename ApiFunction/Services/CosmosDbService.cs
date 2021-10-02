using Azure;
using ApiFunctions.Configurations;
using ApiFunctions.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiFunctions.Models;
using System.Net.Http;

namespace ApiFunctions.Services
{
    public abstract class CosmosDbService<T> : ICosmosDbService<T> where T : BaseEntity
    {
        protected readonly CosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;

        public abstract string ContainerName { get; }

        public CosmosDbService(IOptions<CosmosDbConfiguration> cosmosDbConfiguration,CosmosClient client)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration.Value;
            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                Container container = GetContainer();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse;
            }
            catch (CosmosException ex)
            {

                if (((int)ex.StatusCode) != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return default(T);
            }
        }

        public async Task<T> DeleteAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                await container.DeleteItemAsync<T>(entityId, PartitionKey.None);
            }
            catch (CosmosException ex)
            {

                if (((int)ex.StatusCode) != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
            return default(T);
        }

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();

                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, PartitionKey.None);
                return entityResult;
            }
            catch (CosmosException ex)
            {

                if (((int)ex.StatusCode) != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return default(T);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                Container container = GetContainer();
                ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entity.id.ToString(), PartitionKey.None);
                if (entityResult != null)
                {
                    if (entity is Painting)
                    {

                        var requestOptions = new ItemRequestOptions
                        {
                            IfNoneMatchEtag = entity._etag
                        };
                        await container.ReplaceItemAsync(entity, entity.id.ToString(), PartitionKey.None, requestOptions);
                    }
                    else
                        await container.ReplaceItemAsync(entity, entity.id.ToString(), PartitionKey.None);
                }
                return entity;
            }
            catch (CosmosException ex)
            {

                if (((int)ex.StatusCode) != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                Container container = GetContainer();
                FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
                List<T> entities = new List<T>();
                while(queryResultSetIterator.HasMoreResults)
                {
                    foreach (var entity in await queryResultSetIterator.ReadNextAsync())
                    {
                        entities.Add(entity);
                    }
                }

                return entities;

            }
            catch (CosmosException ex)
            {

                if (((int)ex.StatusCode) != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }


        protected Container GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            var container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
