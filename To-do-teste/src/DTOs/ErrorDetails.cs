using System.Text.Json;

namespace To_do_teste.src.DTOs
{
    public record ErrorDetails(
        int StatusCode,
        string Message
    )
    {
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
