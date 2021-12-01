using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent2021.Shared
{
    public static class Input
    {
        public static string GetInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            Stream inputStream;

            inputStream = GetEmbeddedStream(input) ?? GetFileStream(input);
            if (inputStream == null)
            {
                Console.WriteLine("reading Input directly as passed");
                return input;
            }

            using (var sr = new StreamReader(inputStream))
            {
                return sr.ReadToEnd();
            }
        }

        private static Stream GetFileStream(string input)
        {
            string path = null;
            if (File.Exists(input)) path = input;
            else
            {
                var callingClass = GetCallingType();
                var folder = callingClass.Assembly.Location.Replace("file:///", "").Replace("Advent2021.dll", "");
                var dir = new DirectoryInfo(folder).Parent.Parent.Parent;
                var nameSpace = callingClass.Namespace.Split(".");

                string[] options = new string[6];
                options[0] = Path.Join(dir.ToString(), nameSpace[1], input);
                options[1] = Path.Join(dir.ToString(), nameSpace[1], input + ".txt");
                options[2] = Path.Join(dir.ToString(), input);
                options[3] = Path.Join(dir.ToString(), input + ".txt");
                options[4] = Path.Join(folder, input);
                options[5] = Path.Join(folder, input + ".txt");

                for (int n = 0; n < 6; n++) if (File.Exists(options[n])) { path = options[n]; break; }
            }

            if (path == null) return null;

            return new FileStream(path, FileMode.Open);
        }

        private static Stream GetEmbeddedStream(string input)
        {
            string name = null;
            var resourceNames = typeof(Input).Assembly.GetManifestResourceNames();
            if (resourceNames.Contains(input))
            {
                name = input;
            }
            else
            {
                var callingClass = GetCallingType();
                var option = callingClass.Namespace + "." + input;
                if (resourceNames.Contains(option)) name = option;
                else
                {
                    option = callingClass.Namespace + "." + input + ".txt";
                    if (resourceNames.Contains(option)) name = option;
                }
            }

            if (name == null) return null;

            return typeof(Input).Assembly.GetManifestResourceStream(name);
        }

        private static Type GetCallingType()
        {
            var stackTrace = new StackTrace();
            for (int frameCounter = 1; frameCounter < stackTrace.FrameCount; frameCounter++)
            {
                var frame = stackTrace.GetFrame(frameCounter);
                var type = frame.GetMethod().DeclaringType;

                if (type != typeof(Input)) return type;
            }

            return null;
        }

        public static byte[] GetBytes(string input)
        {
            var inputStream = GetEmbeddedStream(input) ?? GetFileStream(input);
            if (inputStream == null)
            {
                return new byte[0];
            }

            byte[] bytes;
            using (var reader = new StreamReader(inputStream))
            {
                bytes = new byte[reader.BaseStream.Length];

                reader.BaseStream.Read(bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public static IEnumerable<string> GetInputLines(string input, char[] splitOn = null)
        {
            var rawInput = GetInput(input);

            var lines = rawInput.Split(Environment.NewLine);

            if (splitOn != null) lines = lines.SelectMany(l => l.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            return lines;
        }

        public static List<long> GetNumberLines(string input, char[] splitOn = null)
        {
            var rawInput = GetInputLines(input, splitOn);

            return rawInput
                .Where(ri => !string.IsNullOrWhiteSpace(ri))
                .Select(long.Parse)
                .ToList();
        }

        public static string[][] GetBlockLines(string input)
        {
            var rawInput = GetInput(input);

            var blocks = rawInput.Split(Environment.NewLine + Environment.NewLine);

            return blocks.Select(block => block.Split(Environment.NewLine)).ToArray();
        }

        public static long[] GetNumbers(string input)
        {
            var rawInput = GetInput(input);

            return rawInput.Select(c => c - 48L).ToArray();
        }

        public static long[] GetNumbers(string input, char[] splitOn)
        {
            var rawInput = GetInput(input);

            var split = rawInput.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

            return split.Select(s => long.Parse(s)).ToArray();
        }
    }
}
