using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Interlace.Shared.IoC;

[PublicAPI]
public sealed class IoCManager
{
    private readonly Dictionary<Type, object> _instances = new();
    private readonly ReaderWriterLockSlim _lock = new();

    private IoCManager()
    {
    }

    public IReadOnlyDictionary<Type, object> RegisteredTypes => _instances;

    public static IoCManager Instance { get; } = new();

    // ReSharper disable once InconsistentNaming
    public void Register<TInterface, TImplementation>(bool overwrite = false) where TImplementation : class, new()
    {
        RegisterInner<TInterface>(new TImplementation(), overwrite);
    }

    public void Register<TInterface>(object instance, bool overwrite = false)
    {
        RegisterInner<TInterface>(instance, overwrite);
    }

    public TInterface Resolve<TInterface>() where TInterface : class
    {
        return Unsafe.As<TInterface>(Resolve(typeof(TInterface)));
    }

    public object Resolve(Type ty)
    {
        _lock.EnterReadLock();

        object? instance;

        try
        {
            if (!_instances.TryGetValue(ty, out instance))
                throw new InvalidOperationException($"Type '{ty}' is not registered");
        }
        finally
        {
            _lock.ExitReadLock();
        }

        return instance;
    }

    public void InjectDependencies(object instance)
    {
        var ty = instance.GetType();

        while (true)
        {
            var depTy = typeof(DependencyAttribute);

            const BindingFlags bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var propertyInfo in ty.GetProperties(bindings))
                if (propertyInfo.CustomAttributes.Any(a => a.AttributeType == depTy))
                    propertyInfo.SetValue(instance, Resolve(propertyInfo.PropertyType));

            foreach (var fieldInfo in ty.GetFields(bindings))
                if (fieldInfo.CustomAttributes.Any(a => a.AttributeType == depTy))
                    fieldInfo.SetValue(instance, Resolve(fieldInfo.FieldType));

            if (ty.BaseType is null)
                break;

            ty = ty.BaseType;
        }
    }

    private void RegisterInner<T>(object instance, bool overwrite)
    {
        _lock.EnterWriteLock();

        try
        {
            var ty = typeof(T);

            if (_instances.ContainsKey(ty) && !overwrite)
                throw new InvalidOperationException($"Type '{ty}' is already registered");

            _instances[ty] = instance;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
