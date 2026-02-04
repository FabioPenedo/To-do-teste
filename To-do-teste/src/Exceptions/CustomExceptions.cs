namespace To_do_teste.src.Exceptions
{
    public class NotFoundException(string message) : Exception(message) { }

    public class BadRequestException(string message) : Exception(message) { }
}
