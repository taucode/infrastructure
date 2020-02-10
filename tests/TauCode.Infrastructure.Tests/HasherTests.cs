using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TauCode.Extensions;
using TauCode.Infrastructure.Cryptography;

namespace TauCode.Infrastructure.Tests
{
    [TestFixture]
    public class HasherTests
    {
        private class Hasher : HasherBase
        {
            public Hasher(string salt)
                : base(salt)
            {
            }

            public Hasher(byte[] salt)
                : base(salt)
            {

            }
        }

        [Test]
        public void GetHash_ShortStreamShortSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = "abc";
            var hasher = new Hasher(salt);

            // Act
            Guid hashGuid;
            byte[] hash;

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("TauCode.Infrastructure.Tests.Resources.Hello.txt"))
            {
                hashGuid = hasher.GetHashGuid(stream);
            }

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("TauCode.Infrastructure.Tests.Resources.Hello.txt"))
            {
                hash = hasher.GetHash(stream);
            }

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(Encoding.ASCII.GetBytes(salt));
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hello.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }

        [Test]
        public void GetHash_LongStreamLongSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = new string('a', 50000);
            var hasher = new Hasher(salt);

            // Act
            Guid hashGuid;
            byte[] hash;

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("TauCode.Infrastructure.Tests.Resources.Hamlet.txt"))
            {
                hashGuid = hasher.GetHashGuid(stream);
            }

            using (var stream = this.GetType().Assembly.GetManifestResourceStream("TauCode.Infrastructure.Tests.Resources.Hamlet.txt"))
            {
                hash = hasher.GetHash(stream);
            }

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(Encoding.ASCII.GetBytes(salt));
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hamlet.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }

        [Test]
        public void GetHash_ShortStringShortSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = Encoding.ASCII.GetBytes("abc");
            var hasher = new Hasher(salt);

            // Act
            var str = this.GetType().Assembly.GetResourceText("Hello.txt", true);
            var hashGuid = hasher.GetHashGuid(str);
            var hash = hasher.GetHash(str);

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(salt);
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hello.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }

        [Test]
        public void GetHash_LongStringLongSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = Encoding.ASCII.GetBytes(new string('a', 50000));
            var hasher = new Hasher(salt);

            // Act
            var str = this.GetType().Assembly.GetResourceText("Hamlet.txt", true);
            var hashGuid = hasher.GetHashGuid(str);
            var hash = hasher.GetHash(str);

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(salt);
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hamlet.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }

        [Test]
        public void GetHash_ShortArrayShortSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = Encoding.ASCII.GetBytes("abc");
            var hasher = new Hasher(salt);

            // Act
            var bytes = Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hello.txt", true));
            var hashGuid = hasher.GetHashGuid(bytes);
            var hash = hasher.GetHash(bytes);

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(salt);
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hello.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }

        [Test]
        public void GetHash_LongArrayLongSalt_ReturnsValidHashGuid()
        {
            // Arrange
            var salt = Encoding.ASCII.GetBytes(new string('a', 50000));
            var hasher = new Hasher(salt);

            // Act
            var bytes = Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hamlet.txt", true));
            var hashGuid = hasher.GetHashGuid(bytes);
            var hash = hasher.GetHash(bytes);

            // Assert
            var buffer = new List<byte>();
            buffer.AddRange(salt);
            buffer.AddRange(Encoding.ASCII.GetBytes(this.GetType().Assembly.GetResourceText("Hamlet.txt", true)));

            Guid expectedHashGuid;
            byte[] expectedHash;
            using (var algorithm = SHA256.Create())
            {
                expectedHash = algorithm.ComputeHash(buffer.ToArray());
                expectedHashGuid = new Guid(expectedHash.Take(16).ToArray());
            }

            CollectionAssert.AreEqual(expectedHash, hash);
            Assert.That(hashGuid, Is.EqualTo(expectedHashGuid));
        }
    }
}
