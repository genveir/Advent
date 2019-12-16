using Advent2019.Shared;
using Advent2019.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2019.Advent16
{
    public class Solution : ISolution
    {
        public long[] startProgram;
        public long[] program;

        public Solution(Input.InputMode inputMode, string input)
        {
            startProgram = Input.GetInputLines(inputMode, input).ToArray()[0].ToCharArray().Select(c => c - 48).Select(c => (long)c).ToArray();
            Reset();
        }
        public Solution() : this(Input.InputMode.Embedded, "Input") { }

        public void Reset()
        {
            program = startProgram.DeepCopy();
        }

        public long[] RunPhase()
        {
            var newProgram = new long[program.Length];

            newProgram[program.Length - 1] = program[program.Length - 1];
            for (int positionConsidered = program.Length - 2; positionConsidered >= 0; positionConsidered--)
            {
                newProgram[positionConsidered] = (program[positionConsidered] + newProgram[positionConsidered + 1]) % 10;
            }
            return newProgram;
        }

        bool Verbose = true;

        public long[] pattern = new long[] { 0, 1, 0, -1 };

        public string GetResult1()
        {
            Verbose = false;
            for (int n = 0; n < 100; n++)
            {
                program = RunPhase();
                Console.Write(n.ToString().PadRight(4) + ": ");
                Console.WriteLine(ProgramString(program.Length -32, program.Length));
            }

            var result = ProgramString(0, 8);
            return result;
        }

        public string ProgramString(int offset, int length)
        {
            return new string(program.Skip(offset).Take(length).Select(c => c + 48).Select(c => (char)c).ToArray());
        }

        public string GetResult2()
        {
            Reset();

            var newProgram = new long[program.Length * 10000];
            for (int n = 0; n < 10000; n++)
            {
                Array.Copy(program, 0, newProgram, n * program.Length, program.Length);
            }

            program = newProgram;

            var offset = int.Parse(ProgramString(0, 7).TrimStart('0'));
            var fromEnd = program.Length - offset;
            var toDo = fromEnd * 2;

            newProgram = new long[toDo];
            Array.Copy(program, program.Length - toDo, newProgram, 0, toDo);
            program = newProgram;

            for (int n = 0; n < 100; n++)
            {
                program = RunPhase();
                Console.Write(".");
            }
            Console.WriteLine();

            var result = ProgramString(fromEnd, 8);
            return result;
        }

        // observatie: 
        // laatste character verandert nooit, periode daarvoor is kort, daarvoor langer, dan oplopend
        // offset is 5979191 op totale lengte 6500000
        // patroon aan het einde is niet afhankelijk van de lengte van het programma
        // patroon aan het einde is niet afhankelijk van het begin van het programma (??)
        // 00000000000000000000000000000595 en 80871224585914546619083218645595 hebben de zelfde 3 eindcharacters
        // 99999999999999999999999999999595 ook
        // 595 niet
        // 45595 wel (!)

        // 1 is genoeg voor 1
        // 0 is te weinig voor 1

        // 3 is genoeg voor 2
        // 2 is te weinig voor 2

        // 5 chars is genoeg voor 3
        // 4 is te weinig voor 3

        // 7 is genoeg voor 4
        // 6 is te weinig voor 4

        // 2 * n - 1 is genoeg voor n

        // begin: 945/895/245
        // einde: 4295/0645/5595

        // bij de testinputs voor part2 is de offset 90% van de program length ofzo
        // similar voor het echte probleem

        // dan begin je in -1 land. hoe vaak doe je -1 en hoe vaak doe je 1?

        // Patroon!!
        // laatste blijft gelijk
        // eennalaatste is wat hij was + laatste % 10
        // YESSS geldt voor de rest. lineair.
        // geeeen idee hoe dit met de pattern te maken heeft, maar is regelmatig dus het zal wel kloppen.


        // ENGLISH TRANSLATION OF OBSERVATIONS:

        // observation: 
        // last character never changes, period in front is short, longer in front, then increasing
        // offset is 5979191 out of total length 6500000
        // pattern at the end is not dependent on the length of the program
        // pattern at the end is not dependent on the start of the program (??)
        // 00000000000000000000000000000595 and 80871224585914546619083218645595 have the same 3 characters at the end
        // 99999999999999999999999999999595 too
        // 595 doesn't
        // 45595 does (!)

        // 1 is enough for 1
        // 0 is too little for 1

        // 3 is enough for 2
        // 2 is too little for 2

        // 5 is enough for 3
        // 4 is too little for 3

        // 7 is enough for 4
        // 6 is too little for 4

        // 2 * n - 1 is enough for n

        // start: 945/895/245
        // end: 4295/0645/5595

        // at the test inputs for part2 the offset is about 90% of the program length
        // similar for the real problem

        // so you start in -1 land. how many times do you do -1 and how many times do you do 1?

        // Pattern!!
        // last stays the same
        // second to last is what it was + last % 10
        // YESSS goes for the rest. linear.
        // noooo idea what this has to do with the pattern, but it's consistent so it's probably right.
    }
}
