namespace Inz.Helpers
{
    public static class PasswordHashHelper
    {
        public static string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ValidatePassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
