using System.ComponentModel.DataAnnotations;
using To_do_teste.src.Configurations;

namespace To_do_teste.src.DTOs
{
    public record CreateTaskRequest(
        [Required]
        [NotEmptyOrWhiteSpace(ErrorMessage = "O título não pode estar vazio")]
        string Title,

        [Required]
        [NotEmptyOrWhiteSpace(ErrorMessage = "A descrição não pode estar vazia")]
        string Description,

        bool IsCompleted,

        [Required]
        [NotEmptyOrWhiteSpace(ErrorMessage = "A categoria é obrigatória")]
        string Category,

        [Range(1, int.MaxValue)]
        int UserId
    );


    public record UpdateTaskRequest(
        [NotEmptyOrWhiteSpace(ErrorMessage = "O título não pode estar vazio")]
        string? Title,

        [NotEmptyOrWhiteSpace(ErrorMessage = "A descrição não pode estar vazia")]
        string? Description,

        bool? IsCompleted,

        [NotEmptyOrWhiteSpace(ErrorMessage = "A categoria não pode estar vazia")]
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
        string Title,
        string Category
    );
}
