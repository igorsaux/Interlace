using Interlace.Shared.Configuration;

namespace Interlace.Client.Configuration;

public sealed class ConfigurationManager : SharedConfigurationManager
{
    protected override string ConfigFile => "client_config.toml";
}
