namespace Flex;

public class ExceptionErrorHandler : ErrorHandler
{
    public override void ReportError(Token token, string message)
    {
        throw new Exception(message);
    }
}