using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Interlace.Shared.Ecs;

[PublicAPI]
public readonly struct EntityUid : IEquatable<EntityUid>, IComparable<EntityUid>, ISpanFormattable
{
    private const int ClientByte = 0xC;
    private const int ServerByte = 0x0;

    public readonly Guid Guid;

    public static readonly EntityUid Invalid = new(Guid.Empty);

    public EntityUid(Guid guid)
    {
        Guid = guid;
    }

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EntityUid Random(bool isClientSide = false)
    {
        unsafe
        {
            var bytes = stackalloc byte[16];

            System.Random.Shared.NextBytes(new Span<byte>(bytes, 16));

            if (isClientSide)
                bytes[0] = ClientByte;
            else
                bytes[0] = ServerByte;

            return new EntityUid(*(Guid*)bytes);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsClientSide()
    {
        unsafe
        {
            fixed (void* bytes = &Guid)
            {
                return ((byte*)bytes)[0] == ClientByte;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsServerSide()
    {
        unsafe
        {
            fixed (void* bytes = &Guid)
            {
                return ((byte*)bytes)[0] == ServerByte;
            }
        }
    }

    public bool Valid => IsValid();

    /// <summary>
    ///     Creates an entity UID by parsing a string number.
    /// </summary>
    public static EntityUid Parse(ReadOnlySpan<char> uid)
    {
        return new EntityUid(Guid.Parse(uid));
    }

    public static bool TryParse(ReadOnlySpan<char> uid, out EntityUid entityUid)
    {
        if (Guid.TryParse(uid, out var guid))
        {
            entityUid = new EntityUid(guid);
            return true;
        }

        entityUid = Invalid;
        return false;
    }

    [Pure]
    public bool IsValid()
    {
        return Guid != Guid.Empty;
    }

    /// <inheritdoc />
    public bool Equals(EntityUid other)
    {
        return Guid == other.Guid;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is EntityUid id && Equals(id);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Guid.GetHashCode();
    }

    /// <summary>
    ///     Check for equality by value between two objects.
    /// </summary>
    public static bool operator ==(EntityUid a, EntityUid b)
    {
        return a.Guid == b.Guid;
    }

    /// <summary>
    ///     Check for inequality by value between two objects.
    /// </summary>
    public static bool operator !=(EntityUid a, EntityUid b)
    {
        return !(a == b);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Guid.ToString();
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString();
    }

    public bool TryFormat(
        Span<char> destination,
        out int charsWritten,
        ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        return Guid.TryFormat(destination, out charsWritten);
    }

    /// <inheritdoc />
    public int CompareTo(EntityUid other)
    {
        return Guid.CompareTo(other.Guid);
    }
}
