using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuAdapterLimitsExtensions
{
    public static VideoAdapterLimits ToVideoAdapterLimits(this WebGpu.Limits value)
    {
        return new VideoAdapterLimits((int)value.MaxTextureDimension1D, (int)value.MaxTextureDimension2D,
            (int)value.MaxTextureDimension3D, (int)value.MaxTextureArrayLayers, (int)value.MaxBindGroups,
            (int)value.MaxBindingsPerBindGroup, (int)value.MaxDynamicUniformBuffersPerPipelineLayout,
            (int)value.MaxDynamicStorageBuffersPerPipelineLayout, (int)value.MaxSampledTexturesPerShaderStage,
            (int)value.MaxSamplersPerShaderStage, (int)value.MaxStorageBuffersPerShaderStage,
            (int)value.MaxStorageTexturesPerShaderStage, (int)value.MaxUniformBuffersPerShaderStage,
            (long)value.MaxUniformBufferBindingSize, (long)value.MaxStorageBufferBindingSize,
            (int)value.MinUniformBufferOffsetAlignment, (int)value.MinStorageBufferOffsetAlignment,
            (int)value.MaxVertexBuffers, (long)value.MaxBufferSize, (int)value.MaxVertexAttributes,
            (int)value.MaxVertexBufferArrayStride, (int)value.MaxInterStageShaderComponents,
            (int)value.MaxInterStageShaderVariables, (int)value.MaxColorAttachments,
            (int)value.MaxColorAttachmentBytesPerSample, (int)value.MaxComputeWorkgroupStorageSize,
            (int)value.MaxComputeInvocationsPerWorkgroup, (int)value.MaxComputeWorkgroupSizeX,
            (int)value.MaxComputeWorkgroupSizeY, (int)value.MaxComputeWorkgroupSizeZ,
            (int)value.MaxComputeWorkgroupsPerDimension);
    }

    public static WebGpu.Limits ToWebGpuLimits(this VideoAdapterLimits value)
    {
        return new WebGpu.Limits
        {
            MaxTextureDimension1D = (uint)value.MaxTextureDimension1D,
            MaxTextureDimension2D = (uint)value.MaxTextureDimension2D,
            MaxTextureDimension3D = (uint)value.MaxTextureDimension3D,
            MaxTextureArrayLayers = (uint)value.MaxTextureArrayLayers,
            MaxBindGroups = (uint)value.MaxBindGroups,
            MaxBindingsPerBindGroup = (uint)value.MaxBindingsPerBindGroup,
            MaxDynamicUniformBuffersPerPipelineLayout = (uint)value.MaxDynamicUniformBuffersPerPipelineLayout,
            MaxDynamicStorageBuffersPerPipelineLayout = (uint)value.MaxDynamicStorageBuffersPerPipelineLayout,
            MaxSampledTexturesPerShaderStage = (uint)value.MaxSampledTexturesPerShaderStage,
            MaxSamplersPerShaderStage = (uint)value.MaxSamplersPerShaderStage,
            MaxStorageBuffersPerShaderStage = (uint)value.MaxStorageBuffersPerShaderStage,
            MaxUniformBuffersPerShaderStage = (uint)value.MaxUniformBuffersPerShaderStage,
            MaxUniformBufferBindingSize = (ulong)value.MaxUniformBufferBindingSize,
            MaxStorageBufferBindingSize = (ulong)value.MaxStorageBufferBindingSize,
            MinUniformBufferOffsetAlignment = (uint)value.MinUniformBufferOffsetAlignment,
            MinStorageBufferOffsetAlignment = (uint)value.MinStorageBufferOffsetAlignment,
            MaxVertexBuffers = (uint)value.MaxVertexBuffers,
            MaxBufferSize = (uint)value.MaxBufferSize,
            MaxVertexAttributes = (uint)value.MaxVertexAttributes,
            MaxVertexBufferArrayStride = (uint)value.MaxVertexBufferArrayStride,
            MaxInterStageShaderComponents = (uint)value.MaxInterStageShaderComponents,
            MaxInterStageShaderVariables = (uint)value.MaxInterStageShaderVariables,
            MaxColorAttachments = (uint)value.MaxColorAttachments,
            MaxColorAttachmentBytesPerSample = (uint)value.MaxColorAttachmentBytesPerSample,
            MaxComputeWorkgroupStorageSize = (uint)value.MaxComputeWorkgroupStorageSize,
            MaxComputeInvocationsPerWorkgroup = (uint)value.MaxComputeInvocationsPerWorkgroup,
            MaxComputeWorkgroupSizeX = (uint)value.MaxComputeWorkgroupSizeX,
            MaxComputeWorkgroupSizeY = (uint)value.MaxComputeWorkgroupSizeY,
            MaxComputeWorkgroupSizeZ = (uint)value.MaxComputeWorkgroupSizeZ,
            MaxComputeWorkgroupsPerDimension = (uint)value.MaxComputeWorkgroupsPerDimension
        };
    }
}
