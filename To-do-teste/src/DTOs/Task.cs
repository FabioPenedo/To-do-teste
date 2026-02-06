namespace To_do_teste.src.DTOs
{
    public record CreateTaskRequest(
        string Title,
        string Description,
        bool IsCompleted,
        string Category,
        int UserId
    );

    public record UpdateTaskRequest(
        string? Title,
        string? Description,
        bool? IsCompleted,
        string? Category
    );

    public record TaskResponse(
        int Id,
        string Title,
        string Description,
        bool IsCompleted,
        string Category,
        DateTime CreatedAt,
        string UserName
    );

    public record TaskCreatedResponse(
        int Id,
        string Title
    );
}
