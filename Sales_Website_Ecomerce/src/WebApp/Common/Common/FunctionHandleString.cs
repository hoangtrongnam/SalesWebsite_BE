namespace Common
{
    public static class FunctionHandleString
    {
        public static string GetUsernameFromEmail(string email)
        {
            string[] parts = email.Split('@');
            if (parts.Length > 0)
            {
                return parts[0];
            }
            return email;
        }
    }
}
