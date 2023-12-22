using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Advent2023.Shared.InputParsing;

public class ComplexParser
{
    public List<ComplexType> Parse<ComplexType>(IEnumerable<string> inputs)
        => inputs.Select(Parse<ComplexType>).ToList();
    [SingleParseInvokeTarget]
    public ComplexType Parse<ComplexType>(string input)
    {
        var innerParser = GetInputParser<ComplexType>();
        var types = GetConstructorInputTypes<ComplexType>();

        object[] values = new object[0];
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

            object result = generic.Invoke(innerParser, new object[] { input });

            var resultType = result.GetType();

            if (resultType.GetInterfaces().Contains(typeof(ITuple)))
            {
                values = resultType.GetFields().Select(f => f.GetValue(result)).ToArray();
            }
            else
            {
                values = new object[] { result };
            }
        }
        return (ComplexType)Activator.CreateInstance(typeof(ComplexType), values);
    }

    private static InputParser GetInputParser<ComplexType>()
    {
        var constructorToUse = GetConstructor<ComplexType>();

        var attribute = constructorToUse
            .GetCustomAttribute(typeof(ComplexParserConstructorAttribute)) as ComplexParserConstructorAttribute;

        return attribute.InputParser;
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
            .Where(c => c.GetCustomAttribute(typeof(ComplexParserConstructorAttribute)) != null)
            .SingleOrDefault();

        if (constructor == null) throw new NotImplementedException("ComplexParser cannot parse types without a marked constructor");
        return constructor;
    }

    public static bool CanParse(Type type) =>
        type.GetConstructors()
            .Where(c => c.GetCustomAttribute(typeof(ComplexParserConstructorAttribute)) != null)
            .Count() == 1;

    private class SingleParseInvokeTargetAttribute : Attribute
    {
    }
}
