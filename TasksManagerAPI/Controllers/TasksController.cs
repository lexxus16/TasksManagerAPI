// TasksController.cs

using Microsoft.AspNetCore.Mvc;
using TasksManagerAPI.Models;
using TasksManagerAPI.Services;
using System.Collections.Generic;

namespace TasksManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksServices _tasksManager;

        // Constructor to inject the service
        public TasksController(ITasksServices tasksManager)
        {
            _tasksManager = tasksManager;
        }

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TasksModel>> GetAllTasks()
        {
            try
            {
                var tasks = _tasksManager.GetAllTasks();
                return Ok(tasks);  // Return the list of tasks with a 200 OK response
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public ActionResult<TasksModel> GetTaskById(int id)
        {
            try
            {
                var task = _tasksManager.GetTaskById(id);
                return Ok(task);  // Return the task with a 200 OK response
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // POST: api/tasks
        [HttpPost]
        public ActionResult<TasksModel> AddTask([FromBody] TasksModel task)
        {
            if (task == null)
            {
                return BadRequest("Task data is null.");
            }

            try
            {
                var createdTask = _tasksManager.AddTask(task);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.ID }, createdTask);  // Return 201 with the created task
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public ActionResult<TasksModel> UpdateTask(int id, [FromBody] TasksModel task)
        {
            if (task == null)
            {
                return BadRequest("Task data is null.");
            }

            try
            {
                var updatedTask = _tasksManager.UpdateTask(id, task);
                return Ok(updatedTask);  // Return 200 with the updated task
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE: api/tasks/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            try
            {
                _tasksManager.DeleteTask(id);
                return NoContent();  // Return 204 with no content to indicate the task was deleted
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Task with ID {id} not found.");
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
