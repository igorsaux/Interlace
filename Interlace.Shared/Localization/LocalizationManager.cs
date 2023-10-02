using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Interlace.Shared.Application.Hooks;
using Interlace.Shared.Configuration;
using Interlace.Shared.IoC;
using Interlace.Shared.Logging;
using Interlace.Shared.Resources;
using Linguini.Bundle;
using Linguini.Bundle.Builder;
using Linguini.Shared.Types.Bundle;

namespace Interlace.Shared.Localization;

public sealed class LocalizationManager : ILocalizationManager, IInitializeHook
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly ILogManager _log = default!;
    [Dependency] private readonly IResourcesManager _resources = default!;
    
    private readonly Dictionary<string, FluentBundle> _bundles = new();
    private readonly ResourcePath _bundlesFolderPrefix = new("/Localization");
    
    private string _fallbackLanguage = default!;
    private ISawmill _sawmill = default!;

    public void Initialize()
    {
        _sawmill = _log.GetSawmill("loc");

        Language = _cfg.GetValue<string>(CVars.GameLanguage);
        _fallbackLanguage = _cfg.GetValue<string>(CVars.GameFallbackLanguage);
        CultureInfo = new CultureInfo(Language);

        _sawmill.Info("Language: '{0}'", Language);
        _sawmill.Info("Culture info: '{0}'", CultureInfo);

        LoadBundles();
    }

    public string Language { get; private set; } = default!;

    public CultureInfo CultureInfo { get; private set; } = default!;

    public string GetString(string id, params (string key, object? value)[]? args)
    {
        var bundle = _bundles[Language];

        if (TryGetString(id, bundle, out var result, args))
            return result;

        var fallbackBundle = _bundles[_fallbackLanguage];

        if (TryGetString(id, fallbackBundle, out result, args))
            return result;

        _sawmill.Error("String with id '{0}' not found", id);

        return id;
    }

    private void LoadBundles()
    {
        _sawmill.Trace("Loading bundles...");
        _bundles.Clear();

        var bundleDirectories = _resources.GetDirectories(_bundlesFolderPrefix);

        foreach (var bundleDirectory in bundleDirectories)
        {
            var bundleName = bundleDirectory.Path.Split(Path.DirectorySeparatorChar).Last();
            var contents = new List<string>();
            var builder = LinguiniBuilder.Builder().CultureInfo(new CultureInfo(bundleName));

            _sawmill.Trace("Loading bundle '{0}'", bundleName);

            foreach (var file in _resources.GetFiles(bundleDirectory))
            {
                if (!file.Path.EndsWith(".ftl"))
                    continue;

                contents.Add(_resources.ReadTextResource(file));
            }

            var (bundle, errors) = builder.AddResources(contents).Build();

            foreach (var fluentError in errors) _sawmill.Error("{0}", fluentError);

            _bundles.Add(bundleName, bundle);
        }

        _sawmill.Debug("Loaded language bundles: {0}", _bundles.Count);
    }

    private bool TryGetString(string id, FluentBundle bundle, [NotNullWhen(true)] out string? result,
        params (string key, object? value)[]? args)
    {
        Dictionary<string, IFluentType>? fluentArgs = null;

        if (args is not null)
        {
            fluentArgs = new Dictionary<string, IFluentType>(args.Length);

            foreach (var (key, value) in args) fluentArgs.Add(key, new FluentString(value?.ToString()));
        }

        if (!bundle.TryGetMessage(id, fluentArgs, out var errors, out result))
        {
            foreach (var error in errors) _sawmill.Error("{0}", error);

            return false;
        }

        return true;
    }
}
