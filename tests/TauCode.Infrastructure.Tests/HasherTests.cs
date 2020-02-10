using NUnit.Framework;
using System;
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
        }

        [Test]
        public void GetHashGuid_ShortStreamShortSalt_ReturnsValidHashGuid()
        {
            var hasher = new Hasher("abc");
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("TauCode.Infrastructure.Tests.Resources.Hello.txt"))
            {
                var hash = hasher.GetHashGuid(stream);
            }

            throw new NotImplementedException("go on!");
        }
    }
}
