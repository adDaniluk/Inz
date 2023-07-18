namespace Inz.OneOfHelper
{
    public class DatabaseExceptionResponse
    {
        public Exception Exception;

        public DatabaseExceptionResponse(Exception exception) {
            Exception = exception;
        }
    }
}
