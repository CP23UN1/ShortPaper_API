using System;
using System.Text;
using Konscious.Security.Cryptography;
namespace ShortPaper_API.Helper
{

    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            // Generate a salt
            var salt = new byte[16];
            new Random().NextBytes(salt);

            // Hash the password using Argon2
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // Adjust according to your system's capabilities
            argon2.MemorySize = 1024 * 1024; // 1 GiB
            argon2.Iterations = 4;

            // Return the hashed password as a Base64-encoded string
            return Convert.ToBase64String(argon2.GetBytes(32)); // Adjust output size as needed
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            try
            {
                // Decode the hashed password from Base64
                var decodedHash = Convert.FromBase64String(hashedPassword);

                // Hash the provided password using Argon2 with the same parameters
                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
                argon2.Salt = decodedHash.Take(16).ToArray(); // Extract salt from hashed password
                argon2.DegreeOfParallelism = 8;
                argon2.MemorySize = 1024 * 1024;
                argon2.Iterations = 4;

                // Verify if the computed hash matches the stored hash
                return argon2.GetBytes(32).SequenceEqual(decodedHash.Skip(16));
            }
            catch (FormatException)
            {
                // Handle invalid Base64 format
                return false;
            }
            catch (Exception)
            {
                // Handle other exceptions
                return false;
            }
        }
    }

}
