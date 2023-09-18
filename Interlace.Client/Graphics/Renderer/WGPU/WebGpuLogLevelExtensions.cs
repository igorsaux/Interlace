using Interlace.Shared.Logging;
using Interlace.Vendor.WGPU;

namespace Interlace.Client.Graphics.Renderer.WGPU;

internal static class WebGpuLogLevelExtensions
{
    public static WebGpu.LogLevel ToWebGpuLogLevel(this LogLevel value)
    {
        return value switch
        {
            LogLevel.Trace => WebGpu.LogLevel.Trace,
            LogLevel.Debug => WebGpu.LogLevel.Debug,
            LogLevel.Info => WebGpu.LogLevel.Info,
            LogLevel.Warning => WebGpu.LogLevel.Warn,
            LogLevel.Error => WebGpu.LogLevel.Error,
            LogLevel.Fatal => WebGpu.LogLevel.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public static LogLevel ToLogLevel(this WebGpu.LogLevel value)
    {
        return value switch
        {
            WebGpu.LogLevel.Error => LogLevel.Error,
            WebGpu.LogLevel.Warn => LogLevel.Warning,
            WebGpu.LogLevel.Info => LogLevel.Info,
            WebGpu.LogLevel.Debug => LogLevel.Debug,
            WebGpu.LogLevel.Trace => LogLevel.Trace,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
