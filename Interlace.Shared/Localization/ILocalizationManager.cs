using System.Globalization;
using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Localization;

[PublicAPI]
public interface ILocalizationManager : IManager
{
    string Language { get; }

    CultureInfo CultureInfo { get; }

    string GetString(string id, params (string key, object? value)[]? args);
}
