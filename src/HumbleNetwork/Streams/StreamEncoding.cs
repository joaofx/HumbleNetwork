namespace HumbleNetwork.Streams
{
    using System;
    using System.Text;

    public class StreamEncoding
    {
        private static Encoding Default
        {
            get
            {
                if (Type.GetType("Mono.Runtime") != null)
                {
                    return Encoding.UTF8;
                }

                return Encoding.UTF8;
            }
        }

        public static string GetString(byte[] data)
        {
            return Default.GetString(data);
        }

        public static byte[] GetBytes(int data)
        {
            return BitConverter.GetBytes(data);
        }

        public static int GetInt32(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }

        public static string GetString(byte[] data, int index, int count)
        {
            return Default.GetString(data, index, count);
        }

        public static byte[] GetBytes(string delimiter)
        {
            return Default.GetBytes(delimiter);
        }
    }
}