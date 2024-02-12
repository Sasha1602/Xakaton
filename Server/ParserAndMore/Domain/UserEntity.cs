using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Domain;

public class UserEntity
{
    [Key]
    public string Id { get; set; }
    public byte[] Password { get; set; }
    public string Salt { get; set; }
    public string Login { get; set; }

    public void SetPassword(byte[] password)
    {
        Salt = GenerateSalt();
        Password = HashPassword(Encoding.UTF8.GetBytes(password.ToString()), Salt);
    }

    private static string GenerateSalt()
    {
        // Генерация случайной соли
        byte[] saltBytes = new byte[16]; // Например, 16 байт
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    private static byte[] HashPassword(byte[] password, string salt)
    {
        // Комбинирование пароля и соли
        string combined = password + salt;

        // Хеширование пароля с использованием SHA-256
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return hashedBytes;
        }
    }

    public bool VerifyPassword(string enteredPassword)
    {
        var enteredPasswordHash = HashPassword(Encoding.UTF8.GetBytes(enteredPassword), Salt);
        
        return StructuralComparisons.StructuralEqualityComparer.Equals(Password, enteredPasswordHash);
    }
    
    private static char[] ConvertByteArrayToCharArray(byte[] byteArray)
    {
        // Choose an appropriate encoding (e.g., ASCII, UTF-8, etc.)
        Encoding encoding = Encoding.UTF8;

        // Convert byte[] to string
        string resultString = encoding.GetString(byteArray);

        // Convert string to char[]
        char[] charArray = resultString.ToCharArray();

        return charArray;
    }
   
}