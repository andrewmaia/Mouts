using System.Security.Cryptography;
using System.Text;

namespace Mouts.CrossCutting.Utils.Helpers;


public static class CryptoHelper
{

    public static string Encrypt(string plainText, string key)
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        using Aes aes = Aes.Create();
        aes.Key = CreateKey(key, aes.KeySize / 8); 
        aes.GenerateIV(); 

        using MemoryStream ms = new();
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }
    public static string Decrypt(string cipherText, string key)
    {
        if (string.IsNullOrEmpty(cipherText))
            return cipherText;

        byte[] bytes = Convert.FromBase64String(cipherText);

        using Aes aes = Aes.Create();
        aes.Key = CreateKey(key, aes.KeySize / 8);

        byte[] iv = new byte[aes.BlockSize / 8];
        Array.Copy(bytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using MemoryStream ms = new(bytes, iv.Length, bytes.Length - iv.Length);
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    // Garante que a chave tenha o tamanho correto para AES
    private static byte[] CreateKey(string key, int size)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        if (keyBytes.Length == size)
            return keyBytes;

        byte[] finalKey = new byte[size];
        Array.Copy(keyBytes, finalKey, Math.Min(keyBytes.Length, size));
        return finalKey;
    }
}
