namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IOTPGeneratorHelper
    {
        Task<string> GenerateOTP(int length);
    }
    public class OTPGeneratorHelper : IOTPGeneratorHelper
    {
        public async Task<string> GenerateOTP(int length)
        {
            string chars = "0123456789";
            Random random = new Random();

            char[] otp = new char[length];
            for (int i = 0; i < length; i++)
            {
                otp[i] = chars[random.Next(chars.Length)];
            }
                
            return new string(otp);
        }
    }
}
