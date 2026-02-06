using System.ComponentModel.DataAnnotations;

namespace To_do_teste.src.Entities
{
    public class Task
    {
        public int Id { get; private set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; private set; } = null!;

        [MaxLength(500)]
        public string Description { get; private set; } = null!;

        public bool IsCompleted { get; private set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        [Required]
        public int UserId { get; private set; }
        public User User { get; private set; } = null!;

        // Construtor para EF
        protected Task() { }

        public Task(
            string title,
            string description,
            bool isCompleted,
            string category,
            int userId)
        {
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
            Category = category;
            UserId = userId;

            CreatedAt = DateTime.UtcNow;
        }

        public Task(
            int id,
            string title,
            string description,
            bool isCompleted,
            string category)
        {
            Id = id;
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
            Category = category;

            CreatedAt = DateTime.UtcNow;
        }
    }
}
