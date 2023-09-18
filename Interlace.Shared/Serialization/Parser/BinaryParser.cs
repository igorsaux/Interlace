using System.Buffers;
using System.Text;
using CommunityToolkit.HighPerformance;
using Interlace.Shared.IoC;
using Interlace.Shared.Serialization.Node;

namespace Interlace.Shared.Serialization.Parser;

public sealed class BinaryParser
{
    [Dependency] private readonly ISerializationManager _serialization = default!;

    public BinaryParser()
    {
        IoCManager.Instance.InjectDependencies(this);
    }

    public ReadOnlySpan<byte> ToBinary(DataNode? node)
    {
        var buffer = new ArrayBufferWriter<byte>();

        ToBinaryInner(node, buffer);
        
        return buffer.WrittenSpan;
    }

    public DataNode? FromBinary(byte[] data)
    {
        if (data.Length == 0)
            return null;
        
        var reader = new BinaryReader(new MemoryStream(data));

        return ReadValue(reader);
    }

    private static DataNode ReadValue(BinaryReader reader)
    {
        var type = (ValueType)reader.ReadByte();

        switch (type)
        {
            case ValueType.Null:
                return new ValueDataNode(null);
            case ValueType.String:
            {
                var length = reader.ReadInt32();
                var stringBytes = reader.ReadBytes(length);

                return new ValueDataNode(Encoding.UTF8.GetString(stringBytes));
            }
            case ValueType.Byte:
                return new ValueDataNode(reader.ReadByte());
            case ValueType.Short:
                return new ValueDataNode(reader.ReadInt16());
            case ValueType.UShort:
                return new ValueDataNode(reader.ReadUInt16());
            case ValueType.Int:
                return new ValueDataNode(reader.ReadInt32());
            case ValueType.UInt:
                return new ValueDataNode(reader.ReadUInt32());
            case ValueType.Long:
                return new ValueDataNode(reader.ReadInt64());
            case ValueType.ULong:
                return new ValueDataNode(reader.ReadUInt64());
            case ValueType.Mapping:
            {
                var length = reader.ReadInt32();
                var mapping = new MappingDataNode(length);

                for (var i = 0; i < length; i++)
                {
                    var keyNode = (ValueDataNode)ReadValue(reader);
                    var key = (string)keyNode.Value!;
                    var value = ReadValue(reader);
                    
                    mapping.Add(key, value);
                }

                return mapping;
            }
            case ValueType.Sequence:
            {
                var length = reader.ReadInt32();
                var sequence = new SequenceDataNode(length);
                
                for (var i = 0; i < length; i++)
                {
                    sequence.Add(ReadValue(reader));
                }

                return sequence;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void ToBinaryInner(DataNode? node, IBufferWriter<byte> buffer)
    {
        if (node is null)
        {
            buffer.Write(ValueType.Null);
            
            return;
        }
        
        switch (node)
        {
            case ValueDataNode valueNode:
                WriteValue(valueNode.Value, buffer);
                
                break;
            case MappingDataNode mappingNode:
                buffer.Write(ValueType.Mapping);
                buffer.Write(mappingNode.Count);

                foreach (var (key, value) in mappingNode)
                {
                    WriteValue(key, buffer);
                    ToBinaryInner(value, buffer);
                }

                break;
            case SequenceDataNode sequenceNode:
                buffer.Write(ValueType.Sequence);
                buffer.Write(sequenceNode.Count);

                foreach (var value in sequenceNode)
                    ToBinaryInner(value, buffer);

                break;
        }
    }

    private static void WriteValue(object? value, IBufferWriter<byte> buffer)
    {
        if (value is null)
        {
            buffer.Write(ValueType.Null);
            
            return;
        }

        switch (value)
        {
            case byte byteValue:
                buffer.Write(ValueType.Byte);
                buffer.Write(byteValue);
                
                break;
            case short shortValue:
                buffer.Write(ValueType.Short);
                buffer.Write(shortValue);

                break;
            case ushort ushortValue:
                buffer.Write(ValueType.UShort);
                buffer.Write(ushortValue);

                break;
            case int intValue:
                buffer.Write(ValueType.Int);
                buffer.Write(intValue);

                break;
            case uint uintValue:
                buffer.Write(ValueType.UInt);
                buffer.Write(uintValue);

                break;
            case long longValue:
                buffer.Write(ValueType.Long);
                buffer.Write(longValue);

                break;
            case ulong ulongValue:
                buffer.Write(ValueType.ULong);
                buffer.Write(ulongValue);

                break;
            case string stringValue:
                var bytes = Encoding.UTF8.GetBytes(stringValue);
                
                buffer.Write(ValueType.String);
                buffer.Write(bytes.Length);
                buffer.Write(bytes);

                break;
            default:
                throw new NotSupportedException($"Type '{value.GetType()}' is not supported");
        }
    }

    private enum ValueType : byte
    {
        Null = 0,
        String,
        Byte,
        Short,
        UShort,
        Int,
        UInt,
        Long,
        ULong,
        Mapping,
        Sequence
    }
}
