using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AESUtil
{
    private static readonly string Key = "1234567890123456"; // 16����Ʈ (AES-128)

    public static string Encrypt(string plainText)
    {
        byte[] iv = GenerateRandomIV(); // IV ���� ����
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(iv, 0, iv.Length); // IV�� ���� ����
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray()); // Base64 ���ڵ� �� ��ȯ
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] fullCipher = Convert.FromBase64String(cipherText);
        byte[] iv = new byte[16]; // IV ũ��(16����Ʈ)
        Array.Copy(fullCipher, 0, iv, 0, iv.Length); // ����� IV �б�
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
            byte[] iv = new byte[16]; // 16����Ʈ IV ����
            rng.GetBytes(iv);
            return iv;
        }
    }
}
