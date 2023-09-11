namespace Inz.OneOfHelper
{
    public class AlreadyExistResponse
    {
        public string? ResponseMessage { get; set; }
        public AlreadyExistResponse(string message)
        {
           ResponseMessage = message;
        }
        public AlreadyExistResponse()
        {
            
        }
    }
}
