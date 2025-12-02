using UnityEditor;
using System;
using System.Security.Cryptography;

public static class AESKeyGenerator
{
    [MenuItem("Tools/Generate AES Key & IV")]
    private static void Generate()
    {
        using var aes = Aes.Create();
        aes.KeySize = 256; // AES-256
        aes.GenerateKey();
        aes.GenerateIV();

        string keyBase64 = Convert.ToBase64String(aes.Key);
        string ivBase64 = Convert.ToBase64String(aes.IV);

        UnityEngine.Debug.Log($"KeyBase64: {keyBase64}");
        UnityEngine.Debug.Log($"IVBase64:  {ivBase64}");
    }
}
