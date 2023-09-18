using Interlace.Shared.Application.Hooks;
using Interlace.Vendor.Sodium;

namespace Interlace.Shared.Crypto.Sodium;

public sealed class SodiumCryptoManager : ICryptoManager, IInitializeHook
{
    private static long _boxPublicKeyBytes;
    private static long _boxSecretKeyBytes;
    
    public void Initialize()
    {
        var result = LibSodium.Init();

        if (result < 0)
            throw new InvalidOperationException("Can't initialize libsodium");

        _boxPublicKeyBytes = LibSodium.BoxPublicKeyBytes();
        _boxSecretKeyBytes = LibSodium.BoxSecretKeyBytes();
    }

    public (byte[] publicKey, byte[] secretKey) GenerateBoxKeyPair()
    {
        unsafe
        {
            Span<byte> pk = stackalloc byte[(int)_boxPublicKeyBytes];
            Span<byte> sk = stackalloc byte[(int)_boxSecretKeyBytes];

            fixed (byte* pkPtr = pk)
            fixed (byte* skPtr = sk)
            {
                LibSodium.GenerateBoxKeyPair(pkPtr, skPtr);
            }

            return (pk.ToArray(), sk.ToArray());
        }
    }
}
