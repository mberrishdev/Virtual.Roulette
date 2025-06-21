using System.Security.Cryptography;
using System.Text;

namespace Virtual.Roulette.Application.Helpers;

public static class HashHelper
{
    public static string Hash(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = MD5.HashData(bytes);

        StringBuilder sb = new();
        foreach (var t in hashBytes)
            sb.Append(t.ToString("X2"));

        return sb.ToString();
    }
}