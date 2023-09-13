namespace Inz.OneOfHelper
{
    public class NotFoundResponse
    {
        public string? ResponseMessage { get; set; }

        public NotFoundResponse(string response)
        {
            ResponseMessage = response;
        }

        public NotFoundResponse()
        { 

        }
    }
}
