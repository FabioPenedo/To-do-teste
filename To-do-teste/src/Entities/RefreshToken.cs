namespace To_do_teste.src.Entities
{
    public class RefreshToken(Byte[] tokenHash, DateTime expiresAt, int userId)
    {
        public int Id { get; set; }
        public int UserId { get; set; } = userId;
        public User User { get; set; } = null!;
        public Byte[] TokenHash { get; set; } = tokenHash;
        public DateTime ExpiresAt { get; set; } = expiresAt;
        public DateTime? RevokedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
