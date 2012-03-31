namespace HumbleNetwork.Tests.Helpers
{
    using System;
    using System.Text;

    public class StringExtension
    {
        public static string GenerateRandomString(int lenght)
        {
            var randomString = new StringBuilder();
            var randomNumber = new Random();

            for (var i = 0; i < lenght; ++i)
            {
                var appendedChar = Convert.ToChar(Convert.ToInt32(26 * randomNumber.NextDouble()) + 65);
                randomString.Append(appendedChar);
            }

            return randomString.ToString();
        }
    }
}
