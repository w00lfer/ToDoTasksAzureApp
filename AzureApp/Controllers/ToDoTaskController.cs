using AzureApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AzureApp.Models;

namespace AzureApp.Controllers
{
    public class ToDoTaskController : Controller
    {
        private readonly ILogger<ToDoTaskController> _logger;
        private readonly ICosmosDbService _cosmosDbService;
        public ToDoTaskController(ILogger<ToDoTaskController> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Called:: Get Index");
            return View(await _cosmosDbService.GetToDoTasksAsync("SELECT * FROM c"));
        }

        [ActionName("Create")]
        public IActionResult Create()
        {
            _logger.LogInformation("Called:: Get Create");
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("id,Name,Description,ToDoTime,Completed")] ToDoTask toDoTask)
        {
            _logger.LogInformation("Called:: Post Create");
            if (ModelState.IsValid)
            {
                toDoTask.id = Guid.NewGuid().ToString();
                await _cosmosDbService.AddToDoTaskAsync(toDoTask);
                return RedirectToAction("Index");
            }

            return View(toDoTask);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind("id,Name,Description,ToDoTime,Completed")] ToDoTask toDoTask)
        {
            _logger.LogInformation("Called:: Post Edit");
            if (ModelState.IsValid)
            {
                await _cosmosDbService.UpdateToDoTaskAsync(toDoTask.id, toDoTask);
                return RedirectToAction("Index");
            }

            return View(toDoTask);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            _logger.LogInformation("Called:: Get Edit");
            if (id == null)
            {
                return BadRequest();
            }

            ToDoTask toDoTask = await _cosmosDbService.GetToDoTaskAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            _logger.LogInformation("Called:: Get Delete");
            if (id == null)
            {
                return BadRequest();
            }

            ToDoTask toDoTask = await _cosmosDbService.GetToDoTaskAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind("id")] string id)
        {
            _logger.LogInformation("Called:: Post Delete");
            await _cosmosDbService.DeleteToDoTaskAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            _logger.LogInformation("Called:: Get Details");
            return View(await _cosmosDbService.GetToDoTaskAsync(id));
        }
    }
}
