using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private static List<Note> _notes = new List<Note>();
        private static int _nextId = 1;
        private readonly ILogger<NotesController> _logger;

        public NotesController(ILogger<NotesController> logger)
        {
            _logger = logger;
        }

        // GET: api/notes
        [HttpGet]
        public ActionResult<IEnumerable<Note>> GetNotes()
        {
            _logger.LogInformation("Getting all notes");
            return Ok(_notes.OrderByDescending(n => n.CreatedAt));
        }

        // POST: api/notes
        [HttpPost]
        public ActionResult<Note> CreateNote([FromBody] CreateNoteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest(new { error = "Content is required" });
            }

            var note = new Note
            {
                Id = _nextId++,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _notes.Add(note);
            _logger.LogInformation("Created note with ID: {Id}", note.Id);

            return Ok(new
            {
                id = note.Id,
                content = note.Content,
                createdAt = note.CreatedAt
            });
        }

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteNote(int id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note == null)
            {
                _logger.LogWarning("Note with ID {Id} not found", id);
                return NotFound(new { error = "Note not found" });
            }

            _notes.Remove(note);
            _logger.LogInformation("Deleted note with ID: {Id}", id);

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}
