using System.ComponentModel.DataAnnotations;

namespace To_do_teste.src.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public string Category { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = null;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        protected Task() { }

        public Task(string title, string description, string category, int userId, bool isCompleted)
        {
            Title = title;
            Description = description;
            Category = category;
            UserId = userId;
            IsCompleted = isCompleted;
        }

    }
}
