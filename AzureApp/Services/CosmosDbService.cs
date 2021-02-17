using AzureApp.Models;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureApp.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName) 
            => _container = dbClient.GetContainer(databaseName, containerName);

        public async Task AddToDoTaskAsync(ToDoTask task) 
            => await _container.CreateItemAsync<ToDoTask>(task, new PartitionKey(task.id));

        public async Task DeleteToDoTaskAsync(string id) 
            => await _container.DeleteItemAsync<ToDoTask>(id, new PartitionKey(id));

        public async Task<ToDoTask> GetToDoTaskAsync(string id)
        {
            try
            {
                ItemResponse<ToDoTask> response = await this._container.ReadItemAsync<ToDoTask>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ToDoTask>> GetToDoTasksAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<ToDoTask>(new QueryDefinition(queryString));
            List<ToDoTask> results = new List<ToDoTask>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateToDoTaskAsync(string id, ToDoTask toDoTask)
            => await _container.UpsertItemAsync<ToDoTask>(toDoTask, new PartitionKey(id));
    }
}
