using System.Security.Cryptography;
using System.Text;

namespace TauCode.Infrastructure.Cryptography;

public abstract class HasherBase : IHasher
{
    #region Nested

    private class SaltedStream : Stream
    {
        private readonly Stream _innerStream;
        private readonly byte[] _salt;
        private int _pos;

        public SaltedStream(Stream innerStream, byte[] salt)
        {
            _innerStream = innerStream;
            _salt = salt;
        }

        public override void Flush() => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer.Length == 0)
            {
                throw new ArgumentException($"'{nameof(buffer)}' cannot be empty.");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return 0; // nothing to read.
            }

            if (_pos < _salt.Length)
            {
                var totalRead = 0;
                var remainingCount = count - offset;

                if (remainingCount == 0)
                {
                    throw new ArgumentException($"Buffer has not capacity to read to.", nameof(offset));
                }

                // read salt part
                var countToReadFromSalt = Math.Min(_salt.Length - _pos, remainingCount);
                Array.Copy(_salt, _pos, buffer, offset, countToReadFromSalt);

                totalRead += countToReadFromSalt;
                _pos += countToReadFromSalt;
                remainingCount -= countToReadFromSalt;
                offset += countToReadFromSalt;

                if (remainingCount == 0)
                {
                    return totalRead;
                }

                var readFromInnerStream = _innerStream.Read(buffer, offset, remainingCount);
                totalRead += readFromInnerStream;

                return totalRead;
            }
            else
            {
                var readFromInnerStream = _innerStream.Read(buffer, offset, count);
                _pos += readFromInnerStream;

                return readFromInnerStream;
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => _pos;
            set => throw new NotSupportedException();
        }
    }

    #endregion

    #region Fields

    private readonly byte[] _salt;

    #endregion

    #region Constructors

    protected HasherBase(byte[] salt)
    {
        if (salt == null)
        {
            throw new ArgumentNullException(nameof(salt));
        }

        if (salt.Length == 0)
        {
            throw new ArgumentException($"'{nameof(salt)}' cannot be empty.");
        }

        _salt = new byte[salt.Length];
        Array.Copy(salt, 0, _salt, 0, salt.Length);
    }

    protected HasherBase(string salt)
        : this(SaltToBytes(salt))
    {
    }


    #endregion

    #region Private

    private static byte[] SaltToBytes(string salt)
    {
        if (salt == null)
        {
            throw new ArgumentNullException(nameof(salt));
        }

        if (salt.Length == 0)
        {
            throw new ArgumentException($"'{nameof(salt)}' cannot be empty.");
        }

        return Encoding.UTF8.GetBytes(salt);
    }

    private static Guid LeadingBytesToGuid(byte[] bytes)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        if (bytes.Length < 16)
        {
            throw new ArgumentException($"'{nameof(bytes)}' must be at least 16 bytes long.");
        }

        if (bytes.Length == 16)
        {
            return new Guid(bytes);
        }

        var guidBytes = new byte[16];
        Array.Copy(bytes, guidBytes, 16);
        return new Guid(guidBytes);
    }

    #endregion

    #region Protected

    protected virtual HashAlgorithm CreateAlgorithm()
    {
        return SHA256.Create();
    }

    #endregion

    #region IHasher Members

    public byte[] GetHash(string s) =>
        GetHash(Encoding.UTF8.GetBytes(s ?? throw new ArgumentNullException(nameof(s))));

    public byte[] GetHash(byte[] bytes)
    {
        if (bytes == null)
        {
            throw new ArgumentNullException(nameof(bytes));
        }

        var allBytes = new byte[_salt.Length + bytes.Length];
        Array.Copy(_salt, allBytes, _salt.Length);
        Array.Copy(bytes, 0, allBytes, _salt.Length, bytes.Length);

        using (var algorithm = this.CreateAlgorithm())
        {
            return algorithm.ComputeHash(allBytes);
        }
    }

    public byte[] GetHash(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        using (var algorithm = this.CreateAlgorithm())
        using (var saltedStream = new SaltedStream(stream, _salt))
        {
            return algorithm.ComputeHash(saltedStream);
        }
    }

    public Guid GetHashGuid(string s) => LeadingBytesToGuid(this.GetHash(s));

    public Guid GetHashGuid(byte[] bytes) => LeadingBytesToGuid(this.GetHash(bytes));

    public Guid GetHashGuid(Stream stream) => LeadingBytesToGuid(this.GetHash(stream));

    #endregion
}