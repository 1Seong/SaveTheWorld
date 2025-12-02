using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class SecureStorage
{
    // Base64로 저장된 키/IV 를 여기에 넣으세요.
    // (예: Convert.ToBase64String(aes.Key)로 생성한 값)
    private const string keyBase64 = "I5fbJU92HsUgYZsaFH+zCyDPMcnQ2lGAG2TXEPG2CVs=";
    private const string ivBase64 = "GGlcdezQd905/BvRUiR/yA==";

    private static byte[] Key => Convert.FromBase64String(keyBase64);
    private static byte[] IV => Convert.FromBase64String(ivBase64);

    public static string EncryptToBase64(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return string.Empty;

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        aes.Padding = PaddingMode.PKCS7;
        aes.Mode = CipherMode.CBC;

        using var ms = new MemoryStream();
        using var crypto = aes.CreateEncryptor();
        using (var cs = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs, Encoding.UTF8))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string DecryptFromBase64(string base64Cipher)
    {
        if (string.IsNullOrEmpty(base64Cipher)) return string.Empty;

        try
        {
            var cipherBytes = Convert.FromBase64String(base64Cipher);

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            using var ms = new MemoryStream(cipherBytes);
            using var crypto = aes.CreateDecryptor();
            using var cs = new CryptoStream(ms, crypto, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
        catch
        {
            return string.Empty;
        }
    }
}
