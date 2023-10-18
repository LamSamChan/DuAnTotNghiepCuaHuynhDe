using System.Text;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IRandomPasswordHelper
    {
        Task<string> GeneratePassword(int passwordLenght);
    }
    public class RandomPasswordHelper : IRandomPasswordHelper
    {
        public async Task<string> GeneratePassword(int passwordLenght)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var password = new StringBuilder();

            for (int i = 0; i < passwordLenght; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }
            return password.ToString();
        }
    }
}
