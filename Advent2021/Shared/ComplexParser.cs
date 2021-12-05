using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Shared
{
    [AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    sealed class ComplexParserConstructorAttribute : Attribute { }

    public class ComplexParser
    {
        SimpleParser innerParser;

        public ComplexParser(string pattern)
        {
            innerParser = new SimpleParser(pattern);
        }

        public ComplexParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        {
            innerParser = new SimpleParser(startsWithValue, numberOfValues, delimiters);
        }

        public ComplexParser(SimpleParser simpleParser)
        {
            innerParser = simpleParser;
        }

        public List<ComplexType> Parse<ComplexType>(IEnumerable<string> inputs)
            => inputs.Select(Parse<ComplexType>).ToList();
        public ComplexType Parse<ComplexType>(string input)
        {
            var types = GetConstructorInputTypes<ComplexType>();

            object[] values = new object[0];
            if (types.Length != 0)
            {
                var method = typeof(SimpleParser)
                .GetMethods()
                .Where(m => m.Name == nameof(SimpleParser.Parse))
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

        public Type[] GetConstructorInputTypes<ComplexType>()
        {
            var constructorToUse = GetConstructor<ComplexType>();

            var parameters = constructorToUse.GetParameters();
            return parameters.Select(p => p.ParameterType).ToArray();
        }

        private ConstructorInfo GetConstructor<ComplexType>()
        {
            var constructors = typeof(ComplexType).GetConstructors();
            
            var withAttribute = constructors.Where(c => c.GetCustomAttribute(typeof(ComplexParserConstructorAttribute)) != null);
            if (withAttribute.Count() > 0) constructors = withAttribute.ToArray();

            if (constructors.Length == 1) return constructors.Single();
            else return constructors.OrderBy(c => c.GetParameters().Count()).Last(); // more parameters more better
        }
    }
}
