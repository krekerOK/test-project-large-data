namespace TestProject.Utils
{
    public static class StringUtils
    {
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString(int length)
        {
            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[ThreadSafeRandomGenerator.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
