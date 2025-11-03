using System.Security.Cryptography;

namespace WebApi.Tests.Helpers;

internal static class SecureSeedGenerator
{
    public static int GetSecureSeed()
    {
        byte[] bytes = new byte[4];
        RandomNumberGenerator.Fill(bytes);
        return BitConverter.ToInt32(bytes, 0) & int.MaxValue;
    }
}
