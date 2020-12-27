using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2017.OpCode
{
    public class Machine
    {
        public Instruction[] Instructions { get; set; }

        public Register[] Registers { get; set; }

        public Dictionary<string, Register> RegistersByName { get; set; }

        public void Reset()
        {
            foreach (var reg in Registers) reg.Value = 0;
            _stepCounter = 0;
        }

        public void Run()
        {
            for (int n = 0; n < Instructions.Length; n++) Instructions[n].Execute();
        }

        int _stepCounter = 0;
        public void Step()
        {
            Instructions[_stepCounter++].Execute();
        }
    }
}
