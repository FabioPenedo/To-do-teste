namespace To_do_teste.src.Configurations
{
    public static class CookieExtensions
    {
        public static void SetRefreshToken(this HttpResponse response, string refreshToken, int expirationDays)
        {
            response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true em prod
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(expirationDays)
            });
        }

        public static void ClearRefreshToken(this HttpResponse response)
        {
            response.Cookies.Delete("refresh_token");
        }
    }
}
