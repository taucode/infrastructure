using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TauCode.Infrastructure.Cryptography
{
    public abstract class TextTokenGeneratorBase : ITextTokenGenerator
    {
        #region Nested

        private class Hasher : HasherBase
        {
            public Hasher(string salt)
                : base(salt)
            {
            }
        }

        #endregion

        #region Static

        private static readonly HashSet<char> ValueCharsSet;
        private static readonly char[] ValueChars;

        static TextTokenGeneratorBase()
        {

            var list = new List<char>();
            list.AddRange(Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x));
            list.AddRange(Enumerable.Range('A', 'Z' - 'A' + 1).Select(x => (char)x));
            list.AddRange(Enumerable.Range('0', '9' - '0' + 1).Select(x => (char)x));
            list.Add('_');
            ValueChars = list.ToArray();
            ValueCharsSet = new HashSet<char>(ValueChars);
        }

        private static bool IsAcceptableChar(char c)
        {
            return ValueCharsSet.Contains(c);
        }

        #endregion

        #region Fields

        private readonly Random _random;
        private readonly Hasher _hasher;

        #endregion

        #region Constructor

        protected TextTokenGeneratorBase(string valueSalt)
        {
            _hasher = new Hasher(valueSalt);
            _random = new Random();
        }

        #endregion

        #region Private

        private char GenerateRandomAcceptableChar()
        {
            var index = _random.Next(ValueChars.Length);
            return ValueChars[index];
        }

        #endregion

        #region ITextTokenGenerator Members

        public string Generate(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, $"'length' must be a positive number.");
            }

            var itemsToAccomplish = length / 32 + 1;
            byte[] bytes;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                for (var i = 0; i < itemsToAccomplish; i++)
                {
                    var guid = Guid.NewGuid();
                    var hash = _hasher.GetHash(guid.ToByteArray());
                    writer.Write(hash);
                }

                stream.Flush();
                bytes = stream.ToArray();
            }

            var base64Original = Convert.ToBase64String(bytes);
            var sb = new StringBuilder(base64Original);

            for (var i = 0; i < length; i++)
            {
                var c = sb[i];
                if (!IsAcceptableChar(c))
                {
                    sb[i] = this.GenerateRandomAcceptableChar();
                }
            }

            var value = sb.ToString(0, length);
            return value;
        }

        #endregion
    }
}
