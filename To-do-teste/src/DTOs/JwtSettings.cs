namespace To_do_teste.src.DTOs
{
    public record JwtSettings
    {
        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpirationMinutes { get; init; }
        public int ExpirationRefreshDays { get; init; }
        public int MaxSessionsPerUser { get; set; } = 1;
    }
}
