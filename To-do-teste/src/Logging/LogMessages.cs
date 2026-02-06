namespace To_do_teste.src.Logging
{
    public static class LogMessages
    {
        // ==================== JWT ====================

        public static void JwtHeaderMissing(this ILogger logger)
            => logger.LogDebug("auth.jwt.header.missing Request sem Authorization Bearer");

        public static void JwtInvalid(this ILogger logger, Exception ex)
            => logger.LogWarning(ex, "auth.jwt.invalid Access token inválido");

        public static void JwtValid(this ILogger logger)
            => logger.LogDebug("auth.jwt.valid Access token ainda válido");

        public static void JwtExpired(this ILogger logger, DateTime expiredAt)
            => logger.LogInformation("auth.jwt.expired Access token expirado ExpiredAt={ExpiredAt}", expiredAt);

        public static void JwtRefreshStarted(this ILogger logger)
            => logger.LogInformation("auth.jwt.refresh.started Tentando refresh do access token");

        public static void JwtRefreshMissingCookie(this ILogger logger)
            => logger.LogWarning("auth.jwt.refresh.missing_cookie Refresh token não encontrado");

        public static void JwtRefreshSuccess(this ILogger logger)
            => logger.LogInformation("auth.jwt.refresh.success Access token renovado com sucesso");

        public static void JwtRefreshFailed(this ILogger logger, Exception ex)
            => logger.LogError(ex, "auth.jwt.refresh.failed Falha ao renovar access token");

        public static void JwtNewAccessTokenSet(this ILogger logger)
            => logger.LogInformation("auth.jwt.refresh.header_set X-New-Access-Token setado na response");

        // ==================== AUTH ====================

        public static void LoginSuccess(this ILogger logger, int userId)
            => logger.LogInformation("auth.login.success Login realizado com sucesso AuthUserId={AuthUserId}", userId);

        public static void LoginFailed(this ILogger logger, string userName)
            => logger.LogWarning("auth.login.failed Tentativa de login falhou UserName={UserName}", userName);

        // ==================== USER ====================
        public static void UserCreationStarted(this ILogger logger, string userName)
            => logger.LogInformation("user.creation.started Iniciando criação de usuario UserName={UserName}", userName);
        public static void UserCreated(this ILogger logger, int userId, string userName)
            => logger.LogInformation("user.created Usuário criado com sucesso UserId={UserId} UserName={UserName}", userId, userName);

        public static void UserUpdated(this ILogger logger, int userId, string userName)
            => logger.LogInformation("user.updated Usuário atualizado UserId={UserId} UserName={UserName}", userId, userName);

        public static void UserDeleted(this ILogger logger, int userId, string userName)
            => logger.LogInformation("user.deleted Usuário removido UserId={UserId} UserName={UserName}", userId, userName);

        // ==================== Task ====================
        public static void TaskCreationStarted(this ILogger logger, string title)
            => logger.LogInformation("task.creation.started Iniciando criação da task Title={Title}", title);
        public static void TaskCreated(this ILogger logger, string title)
            => logger.LogInformation("task.created Task criada com sucesso Title={Title}", title);

        public static void TaskUpdated(this ILogger logger, string title)
            => logger.LogInformation("task.updated Task atualizada Title={Title}", title);

        public static void TaskDeleted(this ILogger logger, string title)
            => logger.LogInformation("user.deleted Usuário removido Title={Title}", title);

    }
}
