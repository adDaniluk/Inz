namespace Inz.OneOfHelper
{
    public class NotAutorizedResponse
    {
        public string? ReponseMessage { get; set; }

        public NotAutorizedResponse(string message)
        {
            ReponseMessage = message;    
        }

        public NotAutorizedResponse()
        {

        }
    }
}
