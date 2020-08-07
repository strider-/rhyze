using System.Text;

namespace Rhyze.Services
{
    static class Extensions
    {
        public static string ToHexString(this byte[] bytes, bool upperCase = false)
        {
            {
                var result = new StringBuilder(bytes.Length * 2);

                for (int i = 0; i < bytes.Length; i++)
                {
                    result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
                }

                return result.ToString();
            }
        }
    }
}