using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<NotesController> _logger;

        public NotesController(AppDbContext context, ILogger<NotesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            _logger.LogInformation("Getting all notes from database");
            var notes = await _context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
            return Ok(notes);
        }

        // POST: api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] CreateNoteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest(new { error = "Content is required" });
            }

            var note = new Note
            {
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created note with ID: {Id}", note.Id);

            return Ok(new
            {
                id = note.Id.ToString(),
                content = note.Content,
                createdAt = note.CreatedAt
            });
        }

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                _logger.LogWarning("Note with ID {Id} not found", id);
                return NotFound(new { error = "Note not found" });
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted note with ID: {Id}", id);

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}
