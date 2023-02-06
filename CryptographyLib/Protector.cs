using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace CryptographyLib
{
    public static class Protector
    {
        private static readonly byte[] salt = Encoding.Unicode.GetBytes("7BANANAS");
        private static readonly int iterations = 150_000;
        public static string Encrypt(string plainText, string password)
        {
            byte[] encryptedBytes;
            byte[] plainBytes = Encoding.Unicode.GetBytes(plainText);
            using(Aes aes = Aes.Create())
            {
                Stopwatch timer = Stopwatch.StartNew();
                using(Rfc2898DeriveBytes pbkdf2 = 
                    new(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    WriteLine("PBKDF2 algorithm: {0}, Iteration count: {1:N0}", 
                        pbkdf2.HashAlgorithm, pbkdf2.IterationCount);
                    aes.Key = pbkdf2.GetBytes(32);
                    aes.IV = pbkdf2.GetBytes(16);
                }
                timer.Stop();
                WriteLine("{0:N0} milliseconds to generate Key and IV.",
                    arg0: timer.ElapsedMilliseconds);
                WriteLine("Encryption algorithm: {0}-{1}, {2} mode with {3} padding.",
                    "AES", aes.KeySize, aes.Mode, aes.Padding);
                using(MemoryStream ms = new())
                {
                    using(ICryptoTransform transform = aes.CreateEncryptor())
                    {
                        using(CryptoStream cs = new(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(plainBytes, 0, plainBytes.Length);
                            if(!cs.HasFlushedFinalBlock)
                            {
                                cs.FlushFinalBlock();
                            }
                        }
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return ToBase64String(encryptedBytes);
        }
        
        public static string Decrypt(string cipherText, string password)
        {
            byte[] plainBytes, cryptoByte = FromBase64String(cipherText);
            using(Aes aes = Aes.Create()) 
            {
                using(Rfc2898DeriveBytes pbkdf2 = 
                    new(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    aes.Key = pbkdf2.GetBytes(32);
                    aes.IV = pbkdf2.GetBytes(16);
                }
                using (MemoryStream ms = new())
                {
                    using(ICryptoTransform transformer = aes.CreateDecryptor()) 
                    {
                        using(CryptoStream cs = new(ms, 
                            aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cryptoByte, 0, cryptoByte.Length);
                            if(!cs.HasFlushedFinalBlock)
                            {
                                cs.FlushFinalBlock();
                            }
                        }
                    }
                    plainBytes = ms.ToArray();
                }
            }
            return Encoding.Unicode.GetString(plainBytes);
        }

        private static Dictionary<string, User> Users = new();
        public static User Register(string username, string password)
        {
            var rnd = RandomNumberGenerator.Create();
            byte[] saltBytes = new byte[16];
            rnd.GetBytes(saltBytes);
            string saltText = ToBase64String(saltBytes);
            string saltedHashedPassword = SaltAndHashPassword(password, saltText);
            User user = new(username, saltText, saltedHashedPassword);
            Users.Add(user.Name, user);
            return user;  
        }

        public static bool CheckPassword(string username, string password)
        {
            if(!Users.ContainsKey(username))
            {
                return false;
            }

            User usr = Users[username];
            return CheckPassword(password, usr.Salt, usr.SaltedHashedPassword);
        }

        public static bool CheckPassword(string password, string salt, string hashedPassword)
        {
            string saltedHashedPassword = SaltAndHashPassword(password, salt);
            return saltedHashedPassword == hashedPassword;
        }

        private static string SaltAndHashPassword(string password, string salt)
        {
            using(SHA256 sha = SHA256.Create())
            {
                string saltedPassword = password + salt;
                return ToBase64String(
                    sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
            }
        }
    }
}