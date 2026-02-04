using System.ComponentModel.DataAnnotations;

namespace To_do_teste.src.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        protected User() { }

        public User(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }
}
