using System.Security.Cryptography;
using System.Text;

namespace E_BookOnlineBookStore.Utilities
{
    public class ComputeSha256Hash
    {
        /// <summary>
        /// Hash a string using SHA-256.
        /// </summary>
        /// <param name="input">The input string to hash.</param>
        /// <returns>The SHA-256 hash as a hexadecimal string.</returns>
        public string Encrypt(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Compute the hash as a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Converts to a lowercase hex string
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Compare a plain text input with a hashed value to see if they match.
        /// </summary>
        /// <param name="input">The plain text input.</param>
        /// <param name="hashedValue">The previously hashed value.</param>
        /// <returns>True if they match, false otherwise.</returns>
        public bool Verify(string input, string hashedValue)
        {
            // Hash the input and compare it to the hashed value
            string inputHash = Encrypt(input);
            return inputHash.Equals(hashedValue);
        }
    }
}
