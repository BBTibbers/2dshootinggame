using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESUtil
{
    private static readonly string Key = "1234567890123456"; // 16바이트 (AES-128)

    public static string Encrypt(string plainText)
    {
        byte[] iv = GenerateRandomIV(); // IV 랜덤 생성
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length); // IV를 먼저 저장
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray()); // Base64 인코딩 후 반환
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16]; // IV 크기(16바이트)
        Array.Copy(fullCipher, 0, iv, 0, iv.Length); // 저장된 IV 읽기
        byte[] actualCipher = new byte[fullCipher.Length - iv.Length];
        Array.Copy(fullCipher, iv.Length, actualCipher, 0, actualCipher.Length);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream(actualCipher))
            using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }

    private static byte[] GenerateRandomIV()
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] iv = new byte[16]; // 16바이트 IV 생성
            rng.GetBytes(iv);
            return iv;
        }
    }
}
