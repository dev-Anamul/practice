using System.Text;

namespace EncryptionLibrary
{
    public class Encryption
    {
        public static string CaesarCipher(string str, int shift, bool decrypt = false)
        {
            int adjustedShift = decrypt ? (26 - shift) % 26 : shift;
            int newShift = adjustedShift > 0 ? adjustedShift : 26 + (adjustedShift % 26);

            return string.Join("", str.Select((l, i) =>
            {
                int c = str[i];
                if (c >= 65 && c <= 90)
                {
                    // Uppercase letter
                    return Char.ConvertFromUtf32(((c - 65 + newShift) % 26) + 65);
                }
                else if (c >= 97 && c <= 122)
                {
                    // Lowercase letter
                    return Char.ConvertFromUtf32(((c - 97 + newShift) % 26) + 97);
                }
                else
                {
                    return l.ToString();
                }
            }));
        }

        public static string EncryptedInBase64(string str)
        {
            string encryptStr = CaesarCipher(str, 3);
            byte[] bytes = Encoding.ASCII.GetBytes(encryptStr);
            return Convert.ToBase64String(bytes);
        }

        public static string DecryptedInBase64(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            string decryptStr = Encoding.ASCII.GetString(bytes);
            return CaesarCipher(decryptStr, 3, true);
        }
    }
}