using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Interlace.Vendor.WGPU;

[PublicAPI]
public static partial class WebGpu
{
    [PublicAPI]
    public enum AdapterType
    {
        DiscreteGPU = 0x00000000,
        IntegratedGPU = 0x00000001,
        CPU = 0x00000002,
        Unknown = 0x00000003,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum BackendType
    {
        Undefined = 0x00000000,
        Null = 0x00000001,
        WebGPU = 0x00000002,
        D3D11 = 0x00000003,
        D3D12 = 0x00000004,
        Metal = 0x00000005,
        Vulkan = 0x00000006,
        OpenGL = 0x00000007,
        OpenGLES = 0x00000008,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum DeviceLostReason
    {
        Undefined = 0x0,
        Destroyed = 0x1,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum ErrorType
    {
        NoError = 0x00000000,
        Validation = 0x00000001,
        OutOfMemory = 0x00000002,
        Internal = 0x00000003,
        Unknown = 0x00000004,
        DeviceLost = 0x00000005,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum FeatureName
    {
        Undefined = 0x00000000,
        DepthClipControl = 0x00000001,
        Depth32FloatStencil8 = 0x00000002,
        TimestampQuery = 0x00000003,
        PipelineStatisticsQuery = 0x00000004,
        TextureCompressionBC = 0x00000005,
        TextureCompressionETC2 = 0x00000006,
        TextureCompressionASTC = 0x00000007,
        IndirectFirstInstance = 0x00000008,
        ShaderF16 = 0x00000009,
        RG11B10UfloatRenderable = 0x0000000A,
        BGRA8UnormStorage = 0x0000000B,
        Float32Filterable = 0x0000000C,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum LoadOp
    {
        Undefined = 0x00000000,
        Clear = 0x00000001,
        Load = 0x00000002,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum LogLevel
    {
        Off = 0x00000000,
        Error = 0x00000001,
        Warn = 0x00000002,
        Info = 0x00000003,
        Debug = 0x00000004,
        Trace = 0x00000005,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum PowerPreference
    {
        Undefined = 0x00000000,
        LowPower = 0x00000001,
        HighPerformance = 0x00000002,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum PresentMode
    {
        Immediate = 0x00000000,
        Mailbox = 0x00000001,
        Fifo = 0x00000002,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum RenderPassTimestampLocation
    {
        Beginning = 0x00000000,
        End = 0x00000001,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum RequestAdapterStatus
    {
        Success = 0x00000000,
        Unavailable = 0x00000001,
        Error = 0x00000002,
        Unknown = 0x00000003,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum RequestDeviceStatus
    {
        Success = 0x0,
        Error = 0x1,
        Unknown = 0x2,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum StoreOp
    {
        Undefined = 0x00000000,
        Store = 0x00000001,
        Discard = 0x00000002,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum SurfaceType
    {
        Invalid = 0x00000000,
        SurfaceDescriptorFromMetalLayer = 0x00000001,
        SurfaceDescriptorFromWindowsHWND = 0x00000002,
        SurfaceDescriptorFromXlibWindow = 0x00000003,
        SurfaceDescriptorFromCanvasHTMLSelector = 0x00000004,
        ShaderModuleSPIRVDescriptor = 0x00000005,
        ShaderModuleWGSLDescriptor = 0x00000006,
        PrimitiveDepthClipControl = 0x00000007,
        SurfaceDescriptorFromWaylandSurface = 0x00000008,
        SurfaceDescriptorFromAndroidNativeWindow = 0x00000009,
        SurfaceDescriptorFromXcbWindow = 0x0000000A,
        RenderPassDescriptorMaxDrawCount = 0x0000000F,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    public enum TextureFormat
    {
        Undefined = 0x00000000,
        R8Unorm = 0x00000001,
        R8Snorm = 0x00000002,
        R8Uint = 0x00000003,
        R8Sint = 0x00000004,
        R16Uint = 0x00000005,
        R16Sint = 0x00000006,
        R16Float = 0x00000007,
        RG8Unorm = 0x00000008,
        RG8Snorm = 0x00000009,
        RG8Uint = 0x0000000A,
        RG8Sint = 0x0000000B,
        R32Float = 0x0000000C,
        R32Uint = 0x0000000D,
        R32Sint = 0x0000000E,
        RG16Uint = 0x0000000F,
        RG16Sint = 0x00000010,
        RG16Float = 0x00000011,
        RGBA8Unorm = 0x00000012,
        RGBA8UnormSrgb = 0x00000013,
        RGBA8Snorm = 0x00000014,
        RGBA8Uint = 0x00000015,
        RGBA8Sint = 0x00000016,
        BGRA8Unorm = 0x00000017,
        BGRA8UnormSrgb = 0x00000018,
        RGB10A2Unorm = 0x00000019,
        RG11B10Ufloat = 0x0000001A,
        RGB9E5Ufloat = 0x0000001B,
        RG32Float = 0x0000001C,
        RG32Uint = 0x0000001D,
        RG32Sint = 0x0000001E,
        RGBA16Uint = 0x0000001F,
        RGBA16Sint = 0x00000020,
        RGBA16Float = 0x00000021,
        RGBA32Float = 0x00000022,
        RGBA32Uint = 0x00000023,
        RGBA32Sint = 0x00000024,
        Stencil8 = 0x00000025,
        Depth16Unorm = 0x00000026,
        Depth24Plus = 0x00000027,
        Depth24PlusStencil8 = 0x00000028,
        Depth32Float = 0x00000029,
        Depth32FloatStencil8 = 0x0000002A,
        BC1RGBAUnorm = 0x0000002B,
        BC1RGBAUnormSrgb = 0x0000002C,
        BC2RGBAUnorm = 0x0000002D,
        BC2RGBAUnormSrgb = 0x0000002E,
        BC3RGBAUnorm = 0x0000002F,
        BC3RGBAUnormSrgb = 0x00000030,
        BC4RUnorm = 0x00000031,
        BC4RSnorm = 0x00000032,
        BC5RGUnorm = 0x00000033,
        BC5RGSnorm = 0x00000034,
        BC6HRGBUfloat = 0x00000035,
        BC6HRGBFloat = 0x00000036,
        BC7RGBAUnorm = 0x00000037,
        BC7RGBAUnormSrgb = 0x00000038,
        ETC2RGB8Unorm = 0x00000039,
        ETC2RGB8UnormSrgb = 0x0000003A,
        ETC2RGB8A1Unorm = 0x0000003B,
        ETC2RGB8A1UnormSrgb = 0x0000003C,
        ETC2RGBA8Unorm = 0x0000003D,
        ETC2RGBA8UnormSrgb = 0x0000003E,
        EACR11Unorm = 0x0000003F,
        EACR11Snorm = 0x00000040,
        EACRG11Unorm = 0x00000041,
        EACRG11Snorm = 0x00000042,
        ASTC4x4Unorm = 0x00000043,
        ASTC4x4UnormSrgb = 0x00000044,
        ASTC5x4Unorm = 0x00000045,
        ASTC5x4UnormSrgb = 0x00000046,
        ASTC5x5Unorm = 0x00000047,
        ASTC5x5UnormSrgb = 0x00000048,
        ASTC6x5Unorm = 0x00000049,
        ASTC6x5UnormSrgb = 0x0000004A,
        ASTC6x6Unorm = 0x0000004B,
        ASTC6x6UnormSrgb = 0x0000004C,
        ASTC8x5Unorm = 0x0000004D,
        ASTC8x5UnormSrgb = 0x0000004E,
        ASTC8x6Unorm = 0x0000004F,
        ASTC8x6UnormSrgb = 0x00000050,
        ASTC8x8Unorm = 0x00000051,
        ASTC8x8UnormSrgb = 0x00000052,
        ASTC10x5Unorm = 0x00000053,
        ASTC10x5UnormSrgb = 0x00000054,
        ASTC10x6Unorm = 0x00000055,
        ASTC10x6UnormSrgb = 0x00000056,
        ASTC10x8Unorm = 0x00000057,
        ASTC10x8UnormSrgb = 0x00000058,
        ASTC10x10Unorm = 0x00000059,
        ASTC10x10UnormSrgb = 0x0000005A,
        ASTC12x10Unorm = 0x0000005B,
        ASTC12x10UnormSrgb = 0x0000005C,
        ASTC12x12Unorm = 0x0000005D,
        ASTC12x12UnormSrgb = 0x0000005E,
        Force32 = 0x7FFFFFFF
    }

    [PublicAPI]
    [Flags]
    public enum TextureUsage
    {
        None = 0x00000000,
        CopySrc = 0x00000001,
        CopyDst = 0x00000002,
        TextureBinding = 0x00000004,
        StorageBinding = 0x00000008,
        RenderAttachment = 0x00000010,
        Force32 = 0x7FFFFFFF
    }

    public const string LibraryName = "wgpu_native";

    [LibraryImport(LibraryName, EntryPoint = "wgpuCreateInstance")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial IntPtr CreateInstance(ref InstanceDescriptor descriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuInstanceRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr ReleaseInstance(IntPtr instance);

    [LibraryImport(LibraryName, EntryPoint = "wgpuInstanceRequestAdapter")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial IntPtr RequestAdapter(IntPtr instance, RequestAdapterOptions* options,
        delegate* unmanaged[Cdecl]<RequestAdapterStatus, IntPtr, byte*, void*, void> callback, void* userdata);

    [LibraryImport(LibraryName, EntryPoint = "wgpuAdapterRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseAdapter(IntPtr adapter);

    [LibraryImport(LibraryName, EntryPoint = "wgpuInstanceCreateSurface")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CreateSurface(IntPtr instance, ref SurfaceDescriptor surfaceDescriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSurfaceRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseSurface(IntPtr surface);

    [LibraryImport(LibraryName, EntryPoint = "wgpuAdapterRequestDevice")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void RequestDevice(IntPtr adapter, ref DeviceDescriptor descriptor,
        delegate* unmanaged[Cdecl]<RequestDeviceStatus, IntPtr, byte*, void*, void> callback, void* userdata);

    [LibraryImport(LibraryName, EntryPoint = "wgpuDeviceRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseDevice(IntPtr device);

    [LibraryImport(LibraryName, EntryPoint = "wgpuDeviceSetUncapturedErrorCallback")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void SetUncapturedDeviceErrorCallback(IntPtr device,
        delegate* unmanaged[Cdecl]<ErrorType, byte*, void*, void> callback, void* userdata);

    [LibraryImport(LibraryName, EntryPoint = "wgpuDeviceGetQueue")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial IntPtr GetDeviceQueue(IntPtr device);

    [LibraryImport(LibraryName, EntryPoint = "wgpuQueueRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void ReleaseQueue(IntPtr queue);

    [LibraryImport(LibraryName, EntryPoint = "wgpuDeviceCreateSwapChain")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial IntPtr CreateSwapChain(IntPtr device, IntPtr surface,
        ref SwapChainDescriptor descriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSwapChainRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void ReleaseSwapChain(IntPtr swapChain);

    [LibraryImport(LibraryName, EntryPoint = "wgpuAdapterGetLimits")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void GetAdapterLimits(IntPtr adapter, ref SupportedLimits supportedLimits);

    [LibraryImport(LibraryName, EntryPoint = "wgpuAdapterGetProperties")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void GetAdapterProperties(IntPtr adapter, ref AdapterProperties properties);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSetLogLevel")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SetLogLevel(LogLevel level);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSetLogCallback")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void SetLogCallback(delegate* unmanaged[Cdecl]<LogLevel, byte*, void*, void> callback,
        void* userdata);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSurfaceGetPreferredFormat")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial TextureFormat GetPreferredTextureFormat(IntPtr surface, IntPtr adapter);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSwapChainPresent")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void PresentSwapChain(IntPtr swapChain);

    [LibraryImport(LibraryName, EntryPoint = "wgpuSwapChainGetCurrentTextureView")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr GetCurrentSwapChainTextureView(IntPtr swapChain);

    [LibraryImport(LibraryName, EntryPoint = "wgpuTextureViewRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseTextureView(IntPtr textureView);

    [LibraryImport(LibraryName, EntryPoint = "wgpuDeviceCreateCommandEncoder")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CreateCommandEncoder(IntPtr device, ref CommandEncoderDescriptor descriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseCommandEncoder(IntPtr encoder);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderFinish")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr CommandEncoderFinish(IntPtr encoder, ref CommandBufferDescriptor descriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandBufferRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseCommandBuffer(IntPtr commandBuffer);

    [LibraryImport(LibraryName, EntryPoint = "wgpuQueueSubmit")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void SubmitQueue(IntPtr queue, int commandsCount, IntPtr commandsBuffer);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderInsertDebugMarker")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void InsertCommandEncoderDebugMarker(IntPtr encoder, byte* label);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderPushDebugGroup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void PushCommandEncoderDebugGroup(IntPtr encoder, byte* label);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderPopDebugGroup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void PopCommandEncoderDebugGroup(IntPtr encoder);

    [LibraryImport(LibraryName, EntryPoint = "wgpuCommandEncoderBeginRenderPass")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial IntPtr BeginCommandEncoderRenderPass(IntPtr encoder, ref RenderPassDescriptor descriptor);

    [LibraryImport(LibraryName, EntryPoint = "wgpuRenderPassEncoderEnd")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void EndRenderPassEncoder(IntPtr renderPass);

    [LibraryImport(LibraryName, EntryPoint = "wgpuRenderPassEncoderRelease")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void ReleaseRenderPassEncoder(IntPtr renderPass);

    [LibraryImport(LibraryName, EntryPoint = "wgpuRenderPassEncoderInsertDebugMarker")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void InsertRenderPassDebugMarker(IntPtr renderPass, byte* label);

    [LibraryImport(LibraryName, EntryPoint = "wgpuRenderPassEncoderPushDebugGroup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static unsafe partial void PushRenderPassDebugGroup(IntPtr renderPass, byte* label);

    [LibraryImport(LibraryName, EntryPoint = "wgpuRenderPassEncoderPopDebugGroup")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
    public static partial void PopRenderPassDebugGroup(IntPtr renderPass);

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct Color
    {
        public double R;
        public double G;
        public double B;
        public double A;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassColorAttachment
    {
        public IntPtr View;
        public IntPtr ResolveTarget;
        public LoadOp LoadOp;
        public StoreOp StoreOp;
        public Color ClearValue;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassDepthStencilAttachment
    {
        public IntPtr View;
        public LoadOp DepthLoadOp;
        public StoreOp DepthStoreOp;
        public float DepthClearValue;
        public bool DepthReadOnly;
        public LoadOp StencilLoadOp;
        public StoreOp StencilStoreOp;
        public int StencilClearValue;
        public bool StencilReadOnly;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassTimestampWrite
    {
        public IntPtr QuerySet;
        public int QueryIndex;
        public RenderPassTimestampLocation Location;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
        public unsafe byte* Label;
        public int ColorAttachmentsCount;
        public unsafe RenderPassColorAttachment* ColorAttachments;
        public unsafe RenderPassDepthStencilAttachment* DepthStencilAttachment;
        public IntPtr OcclusionQuerySet;
        public int TimestampWriteCount;
        public unsafe RenderPassTimestampWrite* TimestampWrites;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandBufferDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
        public unsafe byte* Label;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct CommandEncoderDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
        public unsafe byte* Label;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct SwapChainDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
        public unsafe byte* Label;
        public TextureUsage Usage;
        public TextureFormat Format;
        public uint Width;
        public uint Height;
        public PresentMode PresentMode;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct ChainedStruct
    {
        public unsafe ChainedStruct* Next;
        public SurfaceType Type;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct InstanceDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RequestAdapterOptions
    {
        public unsafe ChainedStruct* NextInChain;
        public IntPtr CompatibleSurface;
        public PowerPreference PowerPreference;
        public BackendType BackendType;
        public int ForceFallbackAdapter;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceDescriptor
    {
        public unsafe ChainedStruct* NextInChain = null;
        public unsafe byte* Label = null;

        public SurfaceDescriptor()
        {
        }
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct SurfaceDescriptorFromWindowsHWND
    {
        public ChainedStruct Chain;
        public IntPtr HInstance;
        public IntPtr HWND;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct DeviceDescriptor
    {
        public unsafe ChainedStruct* NextInChain;
        public unsafe byte* Label;
        public nuint RequiredFeaturesCount;
        public unsafe FeatureName* RequiredFeatures;
        public unsafe RequiredLimits* RequiredLimits;
        public QueueDescriptor DefaultQueue;
        public unsafe delegate* unmanaged[Cdecl]<DeviceLostReason, byte*, void*, void> DeviceLostCallback;
        public unsafe void* DeviceLostUserdata;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RequiredLimits
    {
        public unsafe ChainedStruct* NextInChain;
        public Limits Limits;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct SupportedLimits
    {
        public unsafe ChainedStruct* NextInChain = null;
        public Limits Limits;

        public SupportedLimits(Limits limits)
        {
            Limits = limits;
        }
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct Limits
    {
        public uint MaxTextureDimension1D;
        public uint MaxTextureDimension2D;
        public uint MaxTextureDimension3D;
        public uint MaxTextureArrayLayers;
        public uint MaxBindGroups;
        public uint MaxBindingsPerBindGroup;
        public uint MaxDynamicUniformBuffersPerPipelineLayout;
        public uint MaxDynamicStorageBuffersPerPipelineLayout;
        public uint MaxSampledTexturesPerShaderStage;
        public uint MaxSamplersPerShaderStage;
        public uint MaxStorageBuffersPerShaderStage;
        public uint MaxStorageTexturesPerShaderStage;
        public uint MaxUniformBuffersPerShaderStage;
        public ulong MaxUniformBufferBindingSize;
        public ulong MaxStorageBufferBindingSize;
        public uint MinUniformBufferOffsetAlignment;
        public uint MinStorageBufferOffsetAlignment;
        public uint MaxVertexBuffers;
        public ulong MaxBufferSize;
        public uint MaxVertexAttributes;
        public uint MaxVertexBufferArrayStride;
        public uint MaxInterStageShaderComponents;
        public uint MaxInterStageShaderVariables;
        public uint MaxColorAttachments;
        public uint MaxColorAttachmentBytesPerSample;
        public uint MaxComputeWorkgroupStorageSize;
        public uint MaxComputeInvocationsPerWorkgroup;
        public uint MaxComputeWorkgroupSizeX;
        public uint MaxComputeWorkgroupSizeY;
        public uint MaxComputeWorkgroupSizeZ;
        public uint MaxComputeWorkgroupsPerDimension;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct AdapterProperties
    {
        public unsafe ChainedStruct* NextInChain;
        public uint VendorID;
        public unsafe byte* VendorName;
        public unsafe byte* Architecture;
        public uint DeviceID;
        public unsafe byte* Name;
        public unsafe byte* DriverDescription;
        public AdapterType AdapterType;
        public BackendType BackendType;
    }

    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct QueueDescriptor
    {
        public unsafe ChainedStruct* NextInChain = null;
        public unsafe byte* Label = null;

        public QueueDescriptor()
        {
        }
    }
}
