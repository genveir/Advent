using Advent2019.OpCode;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Advent2019.Benchmark
{
    public class IntCodeBenchmark
    {
        const string sumOfPrimes = "3,100,1007,100,2,7,1105,-1,87,1007,100,1,14,1105,-1,27,101,-2,100,100,101,1,101,101,1105,1,9,101,105,101,105,101,2,104,104,101,1,102,102,1,102,102,103,101,1,103,103,7,102,101,52,1106,-1,87,101,105,102,59,1005,-1,65,1,103,104,104,101,105,102,83,1,103,83,83,7,83,105,78,1106,-1,35,1101,0,1,-1,1105,1,69,4,104,99";
        const string ackermann = "109,99,21101,0,13,0,203,1,203,2,1105,1,16,204,1,99,1205,1,26,22101,1,2,1,2105,1,0,1205,2,40,22101,-1,1,1,21101,0,1,2,1105,1,16,21101,0,57,3,22101,0,1,4,22101,-1,2,5,109,3,1105,1,16,109,-3,22101,0,4,2,22101,-1,1,1,1105,1,16";
        const string isqrt = "3,1,109,149,21101,0,15,0,20101,0,1,1,1105,1,18,204,1,99,22101,0,1,2,22101,0,1,1,21101,0,43,3,22101,0,1,4,22101,0,2,5,109,3,1105,1,78,109,-3,22102,-1,1,1,22201,1,4,3,22102,-1,1,1,1208,3,0,62,2105,-1,0,1208,3,1,69,2105,-1,0,22101,0,4,1,1105,1,26,1207,1,1,83,2105,-1,0,21101,0,102,3,22101,0,2,4,22101,0,1,5,109,3,1105,1,115,109,-3,22201,1,4,1,21101,0,2,2,1105,1,115,2102,-1,2,140,2101,0,2,133,22101,0,1,2,20001,133,140,1,1207,2,-1,136,2105,-1,0,21201,2,-1,2,22101,1,1,1,1105,1,131";
        const string divmod = "109,366,21101,0,13,0,203,1,203,2,1105,1,18,204,1,204,2,99,1105,0,63,101,166,19,26,1107,-1,366,30,1106,-1,59,101,166,19,39,102,1,58,-1,102,2,58,58,1007,58,0,49,1105,-1,63,101,1,19,19,1105,1,21,1,101,-1,19,19,101,166,19,69,207,1,-1,72,1106,-1,-1,22101,0,1,3,2102,1,2,146,2102,-1,2,152,22102,0,1,1,22102,0,2,2,101,1,19,103,101,-1,103,103,1107,-1,0,107,2105,-1,0,22102,2,2,2,101,166,103,119,207,3,-1,122,1105,-1,144,22101,1,2,2,22102,-1,3,3,101,166,103,137,22001,-1,3,3,22102,-1,3,3,1207,2,-1,149,1105,-1,98,22101,-1,2,2,101,166,103,160,22001,-1,1,1,1105,1,98";
        const string factor = "3,1,109,583,108,0,1,9,1106,-1,14,4,1,99,107,0,1,19,1105,-1,27,104,-1,102,-1,1,1,21101,0,38,0,20101,0,1,1,1105,1,138,2101,1,1,41,101,596,41,45,1101,1,596,77,1101,0,1,53,101,1,77,77,101,1,53,53,7,45,77,67,1105,-1,128,108,1,1,74,1105,-1,128,1005,-1,54,1,53,77,93,7,45,93,88,1105,-1,101,1101,0,1,-1,1,53,93,93,1105,1,83,21101,0,116,0,20101,0,1,1,20101,0,53,2,1105,1,235,1205,2,54,4,53,2101,0,1,1,1105,1,101,108,1,1,133,1105,-1,137,4,1,99,22101,0,1,2,22101,0,1,1,21101,0,163,3,22101,0,1,4,22101,0,2,5,109,3,1105,1,198,109,-3,22102,-1,1,1,22201,1,4,3,22102,-1,1,1,1208,3,0,182,2105,-1,0,1208,3,1,189,2105,-1,0,22101,0,4,1,1105,1,146,1207,1,1,203,2105,-1,0,21101,0,222,3,22101,0,2,4,22101,0,1,5,109,3,1105,1,235,109,-3,22201,1,4,1,21101,0,2,2,1105,1,235,1105,0,280,101,383,236,243,1107,-1,583,247,1106,-1,276,101,383,236,256,102,1,275,-1,102,2,275,275,1007,275,0,266,1105,-1,280,101,1,236,236,1105,1,238,1,101,-1,236,236,101,383,236,286,207,1,-1,289,1106,-1,-1,22101,0,1,3,2102,1,2,363,2102,-1,2,369,22102,0,1,1,22102,0,2,2,101,1,236,320,101,-1,320,320,1107,-1,0,324,2105,-1,0,22102,2,2,2,101,383,320,336,207,3,-1,339,1105,-1,361,22101,1,2,2,22102,-1,3,3,101,383,320,354,22001,-1,3,3,22102,-1,3,3,1207,2,-1,366,1105,-1,315,22101,-1,2,2,101,383,320,377,22001,-1,1,1,1105,1,315";

        public void Run()
        {
            var execSOP = new Executor(sumOfPrimes.Split(","));
            var execACK = new Executor(ackermann.Split(","));
            var execSQRT = new Executor(isqrt.Split(","));
            var execDivMod = new Executor(divmod.Split(","));
            var execFactor = new Executor(factor.Split(","));

            Func<long> testSop100000 = () =>
            {
                execSOP.Reset();
                execSOP.AddInput(100000);

                var t = Run(execSOP);

                Assert.AreEqual(454396537, execSOP.program.output.Dequeue());

                return t;
            };

            Func<long> testAcker36 = () =>
            {
                execACK.Reset();
                execACK.AddInput(3);
                execACK.AddInput(6);

                var t = Run(execACK);

                Assert.AreEqual(509, execACK.program.output.Dequeue());

                return t;
            };

            Func<long> testISQRT = () =>
            {
                execSQRT.Reset();
                execSQRT.AddInput(130);

                var t = Run(execSQRT);

                Assert.AreEqual(11, execSQRT.program.output.Dequeue());

                return t;
            };

            Func<long> testDivMod = () =>
            {
                execDivMod.Reset();
                execDivMod.AddInput(1024);
                execDivMod.AddInput(3);

                var t = Run(execDivMod);

                Assert.AreEqual(341, execDivMod.program.output.Dequeue());
                Assert.AreEqual(1, execDivMod.program.output.Dequeue());

                return t;
            };

            Func<long> testFacSmall = () =>
            {
                execFactor.Reset();
                execFactor.AddInput(2147483647);

                var t = Run(execFactor);

                Assert.AreEqual(2147483647, execFactor.program.output.Dequeue());

                return t;
            };

            Func<long> testFacBig = () =>
            {
                execFactor.Reset();
                execFactor.AddInput(19201644899);

                var t = Run(execFactor);

                Assert.AreEqual(138569, execFactor.program.output.Dequeue());
                Assert.AreEqual(138571, execFactor.program.output.Dequeue());

                return t;
            };

            RunTest("Sum of primes", testSop100000);
            RunTest("Ackermann", testAcker36);
            RunTest("Isqrt", testISQRT);
            RunTest("DivMod", testDivMod);
            RunTest("NoFactors", testFacSmall);
            RunTest("BigFactors", testFacBig);
        }

        private long Run(Executor executor)
        {
            var sw = new Stopwatch();
            sw.Start();
            executor.Execute();
            var t = sw.ElapsedMilliseconds;
            sw.Stop();

            return t;
        }

        private void RunTest(string name, Func<long> func)
        {
            long bestResult = long.MaxValue;
            for (int n = 0; n < 3; n++)
            {
                long result = func();
                if (result < bestResult) bestResult = result;
            }

            Console.WriteLine(name + " " + bestResult + "ms");
        }
    }
}
