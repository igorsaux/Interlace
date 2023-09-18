using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Interlace.Shared.Application;
using JetBrains.Annotations;

namespace Interlace.Shared.Reflection;

[PublicAPI]
public interface IReflectionManager : IManager
{
    List<Type> GetImplementationsOf<TInterface>();

    List<Type> GetImplementationsOf(Type type);

    object CreateInstanceOf<TInterface>(bool injectDependencies = true);

    object CreateInstanceOf(Type type, bool injectDependencies = true);

    Type? FindImplementedInterface<TInterface>(Type type);

    Type? FindImplementedInterface(Type type, Type interfaceType);

    bool HasCustomAttribute<TType, TAttribute>() where TAttribute: Attribute;

    bool HasCustomAttribute<TAttribute>(MemberInfo memberInfo) where TAttribute: Attribute;

    bool TryGetCustomAttribute<TType, TAttribute>([NotNullWhen(true)] out TAttribute? result) where TAttribute: Attribute;
    
    bool TryGetCustomAttribute<TAttribute>(MemberInfo memberInfo, [NotNullWhen(true)] out TAttribute? result) where TAttribute: Attribute;
}
