using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Crypto;

[PublicAPI]
public interface ICryptoManager : IManager
{
    (byte[] publicKey, byte[] secretKey) GenerateBoxKeyPair();
}
