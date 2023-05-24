namespace GetSit.Common
{
    public class PasswordHashing
    {
        public string Encode(string password)
        {
            try
            {
                byte[] EncodeBytes = new byte[password.Length];
                EncodeBytes = System.Text.Encoding.UTF8.GetBytes(password);
                return Convert.ToBase64String(EncodeBytes);

            }
            catch (Exception exception)
            {
                throw new Exception("Error in Encoding: " + exception.Message);
            }
        }

        public string Decode(string encodedpassword)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] decodeBytes = Convert.FromBase64String(encodedpassword);
                int charCount = utf8Decode.GetCharCount(decodeBytes, 0, decodeBytes.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(decodeBytes, 0, decodeBytes.Length, decoded_char, 0);
                return new String(decoded_char);

            }
            catch (Exception exception)
            {
                throw new Exception("Error in Dncoding: " + exception.Message);
            }
        }
    }
}
