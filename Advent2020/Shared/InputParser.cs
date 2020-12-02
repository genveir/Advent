using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Shared
{
    public class InputParser
    {
        private bool startsWithValue;
        private int numberOfValues;
        public int NumberOfValues
        { 
            get { return numberOfValues; } 
            set 
            {
                if (value <= 0) throw new NotImplementedException("can't build a parser for 0 or fewer values");
                if (value > 7) throw new NotImplementedException("can't build a parser for more than 7 values yet");
                numberOfValues = value;
            }
        }
        public string[] delimiters;

        public InputParser(bool startsWithValue, int numberOfValues, IEnumerable<string> delimiters)
        {
            this.startsWithValue = startsWithValue;
            this.NumberOfValues = numberOfValues;
            this.delimiters = delimiters.ToArray();
        }

        public InputParser(string pattern)
        {
            var numberOfValues = 0;
            var delimiters = new List<string>();

            var hasCurrentValue = false;
            var currentDelimiter = new List<char>();
            bool startSet = false;
            foreach (var c in pattern)
            {
                bool isText = false;
                if (c >= 97 && c <= 122) isText = true;
                if (c >= 65 && c <= 90) isText = true;

                if (isText)
                {
                    if (currentDelimiter.Count > 0)
                    {
                        delimiters.Add(new string(currentDelimiter.ToArray()));
                        currentDelimiter = new List<char>();
                    }
                    hasCurrentValue = true;
                    if (!startSet) startsWithValue = true;
                }
                else
                {
                    if (hasCurrentValue)
                    {
                        numberOfValues++;
                        hasCurrentValue = false;
                    }
                    currentDelimiter.Add(c);
                    if (!startSet) startsWithValue = false;
                }

                startSet = true;
            }

            if (currentDelimiter.Count > 0) delimiters.Add(new string(currentDelimiter.ToArray()));
            if (hasCurrentValue) numberOfValues++;

            this.NumberOfValues = numberOfValues;
            this.delimiters = delimiters.ToArray();
        }

        public dynamic Parse(string input)
        {
            Type vtType = GetVTType();

            var instance = Activator.CreateInstance(vtType);

            if (!startsWithValue) input = input.Substring(this.delimiters[0].Length);
            for (int valueIndex = 0; valueIndex < NumberOfValues; valueIndex++)
            {
                int delimiterIndex = valueIndex + (startsWithValue ? 0 : 1);

                string val;
                if (this.delimiters.Length <= delimiterIndex) val = input;
                else
                {
                    var split = input.Split(delimiters[delimiterIndex], 2);
                    val = split[0];
                    input = split[1];
                }
                
                vtType.GetField("Item" + (valueIndex + 1)).SetValue(instance, val);
            }

            return instance;
        }

        private Type GetVTType()
        {
            switch(NumberOfValues)
            {
                case 1: return typeof(ValueTuple<string>);
                case 2: return typeof(ValueTuple<string, string>);
                case 3: return typeof(ValueTuple<string, string, string>);
                case 4: return typeof(ValueTuple<string, string, string, string>);
                case 5: return typeof(ValueTuple<string, string, string, string, string>);
                case 6: return typeof(ValueTuple<string, string, string, string, string, string>);
                case 7: return typeof(ValueTuple<string, string, string, string, string, string, string>);
                default: throw new NotImplementedException("can't go over 7 fields yet");
            }
        }
    }
}

