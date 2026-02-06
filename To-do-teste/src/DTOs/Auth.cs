namespace To_do_teste.src.DTOs
{
    // ================= Request =================
    public record SignupRequest(
        string UserName,
        string Password
    );
    public record LoginRequest(string UserName, string Password);
    public record RefreshRequest(string RefreshToken);

    // ================= Response =================
    public record Tokens(string AccessToken, string RefreshToken, int ExpiresIn);
    public record UserDto(int Id, string Name);
    public record AuthResponse(Tokens token, UserDto User);
}
