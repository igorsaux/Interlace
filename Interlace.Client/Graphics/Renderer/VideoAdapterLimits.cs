using JetBrains.Annotations;

namespace Interlace.Client.Graphics.Renderer;

[PublicAPI]
public sealed class VideoAdapterLimits
{
    public readonly int MaxBindGroups;
    public readonly int MaxBindingsPerBindGroup;
    public readonly long MaxBufferSize;
    public readonly int MaxColorAttachmentBytesPerSample;
    public readonly int MaxColorAttachments;
    public readonly int MaxComputeInvocationsPerWorkgroup;
    public readonly int MaxComputeWorkgroupSizeX;
    public readonly int MaxComputeWorkgroupSizeY;
    public readonly int MaxComputeWorkgroupSizeZ;
    public readonly int MaxComputeWorkgroupsPerDimension;
    public readonly int MaxComputeWorkgroupStorageSize;
    public readonly int MaxDynamicStorageBuffersPerPipelineLayout;
    public readonly int MaxDynamicUniformBuffersPerPipelineLayout;
    public readonly int MaxInterStageShaderComponents;
    public readonly int MaxInterStageShaderVariables;
    public readonly int MaxSampledTexturesPerShaderStage;
    public readonly int MaxSamplersPerShaderStage;
    public readonly long MaxStorageBufferBindingSize;
    public readonly int MaxStorageBuffersPerShaderStage;
    public readonly int MaxStorageTexturesPerShaderStage;
    public readonly int MaxTextureArrayLayers;
    public readonly int MaxTextureDimension1D;
    public readonly int MaxTextureDimension2D;
    public readonly int MaxTextureDimension3D;
    public readonly long MaxUniformBufferBindingSize;
    public readonly int MaxUniformBuffersPerShaderStage;
    public readonly int MaxVertexAttributes;
    public readonly int MaxVertexBufferArrayStride;
    public readonly int MaxVertexBuffers;
    public readonly int MinStorageBufferOffsetAlignment;
    public readonly int MinUniformBufferOffsetAlignment;

    public VideoAdapterLimits(int maxTextureDimension1D, int maxTextureDimension2D, int maxTextureDimension3D,
        int maxTextureArrayLayers, int maxBindGroups, int maxBindingsPerBindGroup,
        int maxDynamicUniformBuffersPerPipelineLayout, int maxDynamicStorageBuffersPerPipelineLayout,
        int maxSampledTexturesPerShaderStage, int maxSamplersPerShaderStage, int maxStorageBuffersPerShaderStage,
        int maxStorageTexturesPerShaderStage, int maxUniformBuffersPerShaderStage, long maxUniformBufferBindingSize,
        long maxStorageBufferBindingSize, int minUniformBufferOffsetAlignment, int minStorageBufferOffsetAlignment,
        int maxVertexBuffers, long maxBufferSize, int maxVertexAttributes, int maxVertexBufferArrayStride,
        int maxInterStageShaderComponents, int maxInterStageShaderVariables, int maxColorAttachments,
        int maxColorAttachmentBytesPerSample, int maxComputeWorkgroupStorageSize, int maxComputeInvocationsPerWorkgroup,
        int maxComputeWorkgroupSizeX, int maxComputeWorkgroupSizeY, int maxComputeWorkgroupSizeZ,
        int maxComputeWorkgroupsPerDimension)
    {
        MaxTextureDimension1D = maxTextureDimension1D;
        MaxTextureDimension2D = maxTextureDimension2D;
        MaxTextureDimension3D = maxTextureDimension3D;
        MaxTextureArrayLayers = maxTextureArrayLayers;
        MaxBindGroups = maxBindGroups;
        MaxBindingsPerBindGroup = maxBindingsPerBindGroup;
        MaxDynamicUniformBuffersPerPipelineLayout = maxDynamicUniformBuffersPerPipelineLayout;
        MaxDynamicStorageBuffersPerPipelineLayout = maxDynamicStorageBuffersPerPipelineLayout;
        MaxSampledTexturesPerShaderStage = maxSampledTexturesPerShaderStage;
        MaxSamplersPerShaderStage = maxSamplersPerShaderStage;
        MaxStorageBuffersPerShaderStage = maxStorageBuffersPerShaderStage;
        MaxStorageTexturesPerShaderStage = maxStorageTexturesPerShaderStage;
        MaxUniformBuffersPerShaderStage = maxUniformBuffersPerShaderStage;
        MaxUniformBufferBindingSize = maxUniformBufferBindingSize;
        MaxStorageBufferBindingSize = maxStorageBufferBindingSize;
        MinUniformBufferOffsetAlignment = minUniformBufferOffsetAlignment;
        MinStorageBufferOffsetAlignment = minStorageBufferOffsetAlignment;
        MaxVertexBuffers = maxVertexBuffers;
        MaxBufferSize = maxBufferSize;
        MaxVertexAttributes = maxVertexAttributes;
        MaxVertexBufferArrayStride = maxVertexBufferArrayStride;
        MaxInterStageShaderComponents = maxInterStageShaderComponents;
        MaxInterStageShaderVariables = maxInterStageShaderVariables;
        MaxColorAttachments = maxColorAttachments;
        MaxColorAttachmentBytesPerSample = maxColorAttachmentBytesPerSample;
        MaxComputeWorkgroupStorageSize = maxComputeWorkgroupStorageSize;
        MaxComputeInvocationsPerWorkgroup = maxComputeInvocationsPerWorkgroup;
        MaxComputeWorkgroupSizeX = maxComputeWorkgroupSizeX;
        MaxComputeWorkgroupSizeY = maxComputeWorkgroupSizeY;
        MaxComputeWorkgroupSizeZ = maxComputeWorkgroupSizeZ;
        MaxComputeWorkgroupsPerDimension = maxComputeWorkgroupsPerDimension;
    }
}
