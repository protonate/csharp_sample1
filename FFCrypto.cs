using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FFCrypto
{

    public static string getMD5Hash(string input)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] bs = Encoding.UTF8.GetBytes(input);
        bs = x.ComputeHash(bs);
        StringBuilder s = new StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        string hash = s.ToString();
        return hash;
    }

    public static string DESEncrypt(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes((string)ConfigurationSettings.AppSettings["CipherKeyProd"]);
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
        StreamWriter writer = new StreamWriter(cryptoStream);
        writer.Write(input);
        writer.Flush();
        cryptoStream.FlushFinalBlock();
        writer.Flush();
        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

    }

    public static string DESDecrypt(string input)
    {
        byte[] bytes = Encoding.ASCII.GetBytes((string)ConfigurationSettings.AppSettings["CipherKeyProd"]);
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream memoryStream = new MemoryStream
                (Convert.FromBase64String(input));
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
        StreamReader reader = new StreamReader(cryptoStream);
        return reader.ReadToEnd();
    }
}