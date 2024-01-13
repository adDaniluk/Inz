namespace Inz.Helpers
{
    public static class ClaimsHelper
    {
        public static int GetUserIdFromClaims(HttpContext context)
        {
            return Int16.Parse(context.User.Claims.First(x => x.Type == "Id").Value);
        }
    }
}
