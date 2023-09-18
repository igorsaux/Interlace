using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Interlace.Vendor.Sodium;

public static partial class LibSodium
{
    private const string LibraryName = "libsodium";
    
    [LibraryImport(LibraryName, EntryPoint = "sodium_init")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial int Init();

    [LibraryImport(LibraryName, EntryPoint = "crypto_box_publickeybytes")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial long BoxPublicKeyBytes();
        
    [LibraryImport(LibraryName, EntryPoint = "crypto_box_secretkeybytes")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial long BoxSecretKeyBytes();        
    
    [LibraryImport(LibraryName, EntryPoint = "crypto_box_keypair")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial int GenerateBoxKeyPair(byte* pk, byte* sk);
}
