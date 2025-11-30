namespace backend.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNoteRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}
