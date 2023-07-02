namespace Inz.OneOfHelper
{
    public class DatabaseException
    {
        public Exception exception;

        public DatabaseException(Exception exception) {
            this.exception = exception;
        }
    }
}
