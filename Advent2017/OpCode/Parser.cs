using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.OpCode
{
    class Parser
    {
        public Dictionary<string, Register> byName;

        public Parser()
        {
            this.byName = new Dictionary<string, Register>();
        }

        public Machine Parse(string[] lines)
        {
            var instructions = new Instruction[lines.Length];
            
            for (int n = 0; n < lines.Length; n++)
            {
                var split = lines[n].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var instructionReg = split[0];
                var instructionType = split[1];
                var instructionParam = split[2];

                var conditionParam1 = split[4];
                var conditionType = split[5];
                var conditionParam2 = split[6];

                Instruction instruction;
                Condition condition;

                switch (conditionType)
                {
                    case ">": condition = new GreaterThan(); break;
                    case "<": condition = new LessThan(); break;
                    case ">=": condition = new GreaterEqual(); break;
                    case "<=": condition = new LessEqual(); break;
                    case "==": condition = new Equal(); break;
                    case "!=": condition = new NotEqual(); break;
                    default: throw new NotImplementedException("unknown condition type " + conditionType);
                }

                condition.parameters = ParseParameters(conditionParam1, conditionParam2);

                switch (instructionType)
                {
                    case "inc": instruction = new Inc(); break;
                    case "dec": instruction = new Dec(); break;
                    default: throw new NotImplementedException("unknown instruction type " + instructionType);
                }

                instruction.register = GetOrAdd(instructionReg);
                instruction.parameters = ParseParameters(instructionParam);
                instruction.condition = condition;

                instructions[n] = instruction;
            }

            return new Machine()
            {
                Instructions = instructions,
                Registers = byName.Values.ToArray(),
                RegistersByName = byName
            };
        }

        public Register GetOrAdd(string registerName)
        {
            Register output;
            if (!byName.TryGetValue(registerName, out output))
            {
                output = new Register() { Name = registerName, Value = 0 };
                byName.Add(registerName, output);
            }
            return output;
        }

        public Func<long>[] ParseParameters(params string[] input)
        {
            return input.Select<string, Func<long>>(i =>
            {
                long value;
                if (long.TryParse(i, out value))
                {
                    return () => value;
                }
                else
                {
                    var register = GetOrAdd(i);
                    return () => register.Value;
                }
            }).ToArray();
        }
    }
}
