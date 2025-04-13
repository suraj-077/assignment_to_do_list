using assignment_to_do_list.Model;
using assignment_to_do_list.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace assignment_to_do_list.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskDbContext _context;

        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
        {
            return Ok(await _context.Tasks.ToListAsync());
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound(new { error = "Task not found" });

            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (taskDto.DueDate.HasValue && taskDto.DueDate.Value < DateTime.UtcNow)
                return BadRequest(new { error = "Due date cannot be in the past." });

            var task = new TaskItem
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                IsCompleted = taskDto.IsCompleted,
                DueDate = taskDto.DueDate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, CreateTaskDto updatedTaskDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound(new { error = "Task not found" });

            if (string.IsNullOrWhiteSpace(updatedTaskDto.Title))
                return BadRequest(new { error = "Title is required" });

            if (updatedTaskDto.DueDate.HasValue && updatedTaskDto.DueDate.Value < DateTime.UtcNow)
                return BadRequest(new { error = "Due date cannot be in the past." });

            task.Title = updatedTaskDto.Title;
            task.Description = updatedTaskDto.Description;
            task.IsCompleted = updatedTaskDto.IsCompleted;
            task.DueDate = updatedTaskDto.DueDate;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        // DELETE: api/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound(new { error = "Task not found" });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task existed and now deleted" });
        }
    }
}
