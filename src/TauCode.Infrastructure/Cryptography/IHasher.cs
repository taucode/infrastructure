using System;
using System.IO;

namespace TauCode.Infrastructure.Cryptography
{
    public interface IHasher
    {
        byte[] GetHash(string s);
        byte[] GetHash(byte[] bytes);
        byte[] GetHash(Stream stream);
        Guid GetHashGuid(string s);
        Guid GetHashGuid(byte[] bytes);
        Guid GetHashGuid(Stream stream);
    }
}
