using System.Collections.Generic;
using System.Threading.Tasks;
using AzureApp.Models;

namespace AzureApp.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<ToDoTask>> GetToDoTasksAsync(string query);
        Task<ToDoTask> GetToDoTaskAsync(string id);
        Task AddToDoTaskAsync(ToDoTask toDoTask);
        Task UpdateToDoTaskAsync(string id, ToDoTask toDoTask);
        Task DeleteToDoTaskAsync(string id);
    }
}
