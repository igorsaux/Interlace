using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interlace.Shared.IoC;

namespace Interlace.Shared.Reflection;

public sealed class ReflectionManager : IReflectionManager
{
    public List<Type> GetImplementationsOf<TInterface>()
    {
        return GetImplementationsOf(typeof(TInterface));
    }

    public List<Type> GetImplementationsOf(Type type)
    {
        var isInterface = type.IsInterface;
        var types = new List<Type>();
        var domain = AppDomain.CurrentDomain;
        var assemblies = domain.GetAssemblies();

        foreach (var assembly in assemblies)
        foreach (var assemblyType in assembly.GetTypes())
        {
            if (isInterface)
            {
                if (FindImplementedInterface(assemblyType, type) is not null)
                {
                    types.Add(assemblyType);
                }
            }
            else
            {
                if (assemblyType.BaseType == type)
                    types.Add(assemblyType);
            }
        }
        
        return types;
    }

    public object CreateInstanceOf<TInterface>(bool injectDependencies = true)
    {
        var instance = Activator.CreateInstance<TInterface>()!;
        
        if (injectDependencies)
            IoCManager.Instance.InjectDependencies(instance);

        return instance;
    }

    public object CreateInstanceOf(Type type, bool injectDependencies = true)
    {
        var instance = Activator.CreateInstance(type)!;
        
        if (injectDependencies)
            IoCManager.Instance.InjectDependencies(instance);

        return instance;
    }

    public Type? FindImplementedInterface<TInterface>(Type type)
    {
        return FindImplementedInterface(type, typeof(TInterface));
    }

    public Type? FindImplementedInterface(Type type, Type interfaceType)
    {
        foreach (var i in type.GetInterfaces())
        {
            if (i.IsGenericType)
            {
                if (i.GetGenericTypeDefinition() == interfaceType)
                    return i;
            }
            else if (i == interfaceType)
                return i;
        }

        return null;
    }

    public bool HasCustomAttribute<TType, TAttribute>() where TAttribute : Attribute
    {
        return HasCustomAttribute<TAttribute>(typeof(TType));
    }

    public bool HasCustomAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute : Attribute
    {
        return memberInfo.GetCustomAttribute<TAttribute>() is not null;
    }

    public bool TryGetCustomAttribute<TType, TAttribute>([NotNullWhen(true)] out TAttribute? result) where TAttribute: Attribute
    {
        return TryGetCustomAttribute(typeof(TType), out result);
    }

    public bool TryGetCustomAttribute<TAttribute>(MemberInfo memberInfo, [NotNullWhen(true)] out TAttribute? result) where TAttribute: Attribute
    {
        result = memberInfo.GetCustomAttribute<TAttribute>();

        return result is not null;
    }
}
