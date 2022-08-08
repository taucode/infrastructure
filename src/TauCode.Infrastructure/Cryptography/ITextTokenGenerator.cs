namespace TauCode.Infrastructure.Cryptography;

public interface ITextTokenGenerator
{
    string Generate(int length);
}