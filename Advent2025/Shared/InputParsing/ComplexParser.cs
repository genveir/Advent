using System.Reflection;
using System.Runtime.CompilerServices;

namespace Advent2025.Shared.InputParsing;

public class ComplexParser
{
    public List<ComplexType> Parse<ComplexType>(IEnumerable<string> inputs)
        => inputs.Select(Parse<ComplexType>).ToList();

    private enum ConstructionType
    { Constructor, FactoryMethod }

    public ComplexType Parse<ComplexType>(string input)
    {
        var constructionType = DetermineConstructionType<ComplexType>();
        var innerParser = GetInputParser<ComplexType>(constructionType);
        var types = GetInputTypes<ComplexType>(constructionType);

        object[] values = [];
        if (types.Length != 0)
        {
            var method = typeof(InputParser)
                .GetMethods()
                .Where(m => m.Name == nameof(InputParser.Parse))
                .Where(m => m.GetParameters().Length == 1)
                .Where(m => m.GetParameters().Single().ParameterType == typeof(string))
                .Where(m => m.GetGenericArguments().Length == types.Length)
                .SingleOrDefault();

            var generic = method.MakeGenericMethod(types);

            object result = generic.Invoke(innerParser, [input]);

            var resultType = result.GetType();

            if (resultType.GetInterfaces().Contains(typeof(ITuple)))
            {
                values = resultType.GetFields().Select(f => f.GetValue(result)).ToArray();
            }
            else
            {
                values = [result];
            }
        }
        return constructionType switch
        {
            ConstructionType.Constructor => (ComplexType)Activator.CreateInstance(typeof(ComplexType), values),
            ConstructionType.FactoryMethod => (ComplexType)GetFactoryMethod<ComplexType>().Invoke(null, values),
            _ => throw new NotImplementedException("unknown construction type")
        };
    }

    private static InputParser GetInputParser<ComplexType>(ConstructionType constructionType)
    {
        var attribute = constructionType switch
        {
            ConstructionType.Constructor => GetConstructor<ComplexType>()
                .GetCustomAttribute<ComplexParserTargetAttribute>(),
            ConstructionType.FactoryMethod => GetFactoryMethod<ComplexType>()
                .GetCustomAttribute<ComplexParserTargetAttribute>(),
            _ => throw new NotImplementedException("unknown construction type")
        };

        return attribute.InputParser;
    }

    private static ConstructionType DetermineConstructionType<ComplexType>()
    {
        if (HasConstructorParsingAttribute(typeof(ComplexType))) return ConstructionType.Constructor;
        if (HasFactoryMethodParsingAttribute(typeof(ComplexType))) return ConstructionType.FactoryMethod;
        throw new NotImplementedException("ComplexParser cannot parse types without a marked factory method or constructor");
    }

    private static Type[] GetInputTypes<ComplexType>(ConstructionType constructionType)
    {
        return constructionType switch
        {
            ConstructionType.Constructor => GetConstructorInputTypes<ComplexType>(),
            ConstructionType.FactoryMethod => GetFactoryMethodInputTypes<ComplexType>(),
            _ => throw new InvalidOperationException("unknown construction type")
        };
    }

    private static Type[] GetConstructorInputTypes<ComplexType>()
    {
        var constructorToUse = GetConstructor<ComplexType>();

        var parameters = constructorToUse.GetParameters();
        return parameters.Select(p => p.ParameterType).ToArray();
    }

    private static ConstructorInfo GetConstructor<ComplexType>()
    {
        var constructor = typeof(ComplexType)
            .GetConstructors()
            .Where(c => c.GetCustomAttribute<ComplexParserTargetAttribute>() != null)
            .SingleOrDefault();

        return constructor ??
            throw new NotImplementedException("ComplexParser cannot parse types without a marked factory method or constructor");
    }

    private static Type[] GetFactoryMethodInputTypes<ComplexType>()
    {
        var methodToUse = GetFactoryMethod<ComplexType>();

        var parameters = methodToUse.GetParameters();
        return parameters.Select(p => p.ParameterType).ToArray();
    }

    private static MethodInfo GetFactoryMethod<ComplexType>()
    {
        var factoryMethod = typeof(ComplexType)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(c => c.GetCustomAttribute<ComplexParserTargetAttribute>() != null)
            .SingleOrDefault();

        return factoryMethod ??
            throw new NotImplementedException("ComplexParser cannot parse types without a marked factory method or constructor");
    }

    public static bool CanParse(Type type) =>
        HasConstructorParsingAttribute(type) ^ HasFactoryMethodParsingAttribute(type);

    public static bool HasConstructorParsingAttribute(Type type) =>
        type.GetConstructors()
            .Where(c => c.GetCustomAttribute<ComplexParserTargetAttribute>() != null)
            .Count() == 1;

    public static bool HasFactoryMethodParsingAttribute(Type type) =>
        type.GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(c => c.GetCustomAttribute<ComplexParserTargetAttribute>() != null)
            .Count() == 1;
}