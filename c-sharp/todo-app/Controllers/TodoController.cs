using Common;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;


namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public TodoController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos([FromQuery] Pagination query)
        {
            IQueryable<Todo> todos = _context.Todos;

            if (!String.IsNullOrEmpty(query.Search))
            {
                todos = todos.Where(t => t.Title.Contains(query.Search));
            }

            if (!String.IsNullOrEmpty(query.Sort))
            {
                if (query.Order == "asc")
                {
                    todos = todos.OrderBy(t => t.GetType().GetProperty(query.Sort).GetValue(t));
                }
                else
                {
                    todos = todos.OrderByDescending(t => t.GetType().GetProperty(query.Sort).GetValue(t));
                }
            }

            if (query.Page == 0)
            {
                query.Page = 1;
            }

            if (query.PageSize == 0)
            {
                query.PageSize = 10;
            }

            if (query.Fields != String.Empty)
            {
                var fields = query.Fields.Split(',');
                var properties = typeof(Todo).GetProperties().Where(p => fields.Contains(p.Name));
                todos = todos.Select(t => new Todo
                {
                    Id = t.Id,
                    Title = t.Title,
                });
            }

            if (query.Filter != String.Empty)
            {
                var filter = query.Filter.Split(',');
                todos = todos.Where(t => filter.Contains(t.Title));
            }

            var result = await todos.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

            return result;

        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById([FromRoute] int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo([FromBody] Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoById", new { id = todo.Id }, todo);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo([FromRoute] int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoById([FromRoute] int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}