using System;
using System.IO;
using System.Security.Cryptography;

namespace Core.Utils
{
  public static class Encryptor
  {
    static readonly byte[] Key = new byte[] { 0x77, 0xC8, 0xC3, 0x07, 0x33, 0xFD, 0xEF,
      0xA5, 0x2A, 0x5A, 0x56, 0x9F, 0xCD, 0x49, 0xC5, 0xCA};

    public static string Decrypt(string decryptValue)
    {
      byte[] cipherTextCombined = Convert.FromBase64String(decryptValue);
      string plaintext = null;

      using (Aes aesAlg = Aes.Create())
      {
        aesAlg.Key = Key;
        byte[] IV = new byte[aesAlg.BlockSize / 8];
        byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];
        Array.Copy(cipherTextCombined, IV, IV.Length);
        Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);
        aesAlg.IV = IV;
        aesAlg.Mode = CipherMode.CBC;
        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        using (var msDecrypt = new MemoryStream(cipherText))
        {
          using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
          {
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
              plaintext = srDecrypt.ReadToEnd();
            }
          }
        }

      }

      return plaintext;
    }

    public static string Encrypt(string strValue)
    {
      byte[] encrypted;
      byte[] IV;
      using (Aes aesAlg = Aes.Create())
      {
        aesAlg.Key = Key;
        aesAlg.GenerateIV();
        IV = aesAlg.IV;
        aesAlg.Mode = CipherMode.CBC;
        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using (var msEncrypt = new MemoryStream())
        {
          using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
          {
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
              swEncrypt.Write(strValue);
            }
            encrypted = msEncrypt.ToArray();
          }
        }
      }

      var combinedIvCt = new byte[IV.Length + encrypted.Length];
      Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
      Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);
      return Convert.ToBase64String(combinedIvCt);
    }
  }
}
