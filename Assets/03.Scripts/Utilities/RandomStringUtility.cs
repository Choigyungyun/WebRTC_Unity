using System;
using System.Text;

namespace MultiPartyWebRTC.Utility
{
    public static class RandomStringUtility
    {
        private static Random random = new Random();
        private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString(int length, bool lastUpdate = true)
        {
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(Characters[random.Next(Characters.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
