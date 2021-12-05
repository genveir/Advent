using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Advent2021.Shared
{
    [AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    sealed class ComplexParserConstructorAttribute : Attribute { }

    public class ComplexParser<ComplexType> where ComplexType : class
    {
        InputParser innerParser;

        public ComplexParser(string pattern)
        {
            innerParser = new InputParser(pattern);
        }

        public ComplexParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        {
            innerParser = new InputParser(startsWithValue, numberOfValues, delimiters);
        }

        public List<ComplexType> Parse(IEnumerable<string> inputs) => inputs.Select(Parse).ToList();
        public ComplexType Parse(string input)
        {
            var types = GetConstructorInputTypes();

            var method = typeof(InputParser)
                .GetMethods()
                .Where(m => m.Name == "Parse")
                .Where(m => m.GetGenericArguments().Length == types.Length)
                .SingleOrDefault();

            var generic = method.MakeGenericMethod(types);

            object result = generic.Invoke(innerParser, new object[] { input });

            var values = result.GetType().GetFields().Select(f => f.GetValue(result)).ToArray();

            return Activator.CreateInstance(typeof(ComplexType), values) as ComplexType;
        }

        public Type[] GetConstructorInputTypes()
        {
            var constructorToUse = GetConstructor();

            var parameters = constructorToUse.GetParameters();
            return parameters.Select(p => p.ParameterType).ToArray();
        }

        private ConstructorInfo GetConstructor()
        {
            var constructors = typeof(ComplexType).GetConstructors();
            
            var withAttribute = constructors.Where(c => c.GetCustomAttribute(typeof(ComplexParserConstructorAttribute)) != null);
            if (withAttribute.Count() > 0) constructors = withAttribute.ToArray();

            if (constructors.Length == 1) return constructors.Single();
            else return constructors.OrderBy(c => c.GetParameters().Count()).Last(); // more parameters more better
        }
    }
}
