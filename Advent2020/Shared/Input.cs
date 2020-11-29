using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Advent2020.Shared
{
    public static class Input
    {
        public static string GetInput(string input)
        {
            Stream inputStream;

            if (GetEmbeddedStream(input) != null) inputStream = GetEmbeddedStream(input);
            else if (GetFileStream(input) != null) inputStream = GetFileStream(input);
            else
            {
                Console.WriteLine("reading input directly");
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
                var folder = callingClass.Assembly.CodeBase.Replace("file:///", "").Replace("Advent2020.dll", "");
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

            Console.WriteLine("reading input from file");
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

            Console.WriteLine("reading input from embedded resource");
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

        public static IEnumerable<string> GetInputLines(string input, char[] splitOn = null)
        {
            if (splitOn == null) splitOn = new char[] { '\r', '\n' };

            var rawInput = GetInput(input);

            return rawInput.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
