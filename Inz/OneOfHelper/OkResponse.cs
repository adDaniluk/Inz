namespace Inz.OneOfHelper
{
    public class OkResponse
    {
        public string? ResponseMessage { get; set; }

        public OkResponse()
        {
        }

        public OkResponse(string response)
        {
            ResponseMessage = response;
        }
    }
}
