using System.ComponentModel.DataAnnotations;

namespace To_do_teste.src.Entities
{
    public class User(string userName, string passwordHash)
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = userName;

        [Required]
        public string PasswordHash { get; set; } = passwordHash;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
