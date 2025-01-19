using System;
using System.Text;

namespace MultiPartyWebRTC.Utility
{
    public static class RandomIntUtility
    {
        private static Random random = new();
        private const string Characters = "0123456789";

        public static string GenerateRandomInt(int length)
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