﻿using System.Diagnostics;

namespace Advent2024.Shared;

public static class Input
{
    public static string GetInput(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Input was empty");
            return input;
        }

        Stream inputStream;

        inputStream = GetEmbeddedStream(input) ?? GetFileStream(input);
        if (inputStream == null)
        {
            Console.WriteLine("reading Input directly as passed");
            return input;
        }

        using var sr = new StreamReader(inputStream);
        var data = sr.ReadToEnd();

        if (string.IsNullOrEmpty(data))
        {
            Console.WriteLine("Input was empty");
        }
        return data;
    }

    private static FileStream GetFileStream(string input)
    {
        string path = null;
        if (File.Exists(input)) path = input;
        else
        {
            var callingClass = GetCallingType();
            var assemblyName = callingClass.Assembly.GetName().Name + ".dll";
            var folder = callingClass.Assembly.Location.Replace("file:///", "").Replace(assemblyName, "");
            var dir = new DirectoryInfo(folder).Parent.Parent.Parent;
            var nameSpace = callingClass.Namespace.Split(".");

            string[] options =
            [
                Path.Join(dir.ToString(), nameSpace[1], input),
                Path.Join(dir.ToString(), nameSpace[1], input + ".txt"),
                Path.Join(dir.ToString(), input),
                Path.Join(dir.ToString(), input + ".txt"),
                Path.Join(folder, input),
                Path.Join(folder, input + ".txt"),
            ];
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
            return [];
        }

        byte[] bytes;
        using (var reader = new StreamReader(inputStream))
        {
            bytes = new byte[reader.BaseStream.Length];

            reader.BaseStream.ReadExactly(bytes);
        }
        return bytes;
    }

    public static List<string> GetInputLines(string input, char[] splitOn = null)
    {
        var rawInput = GetInput(input);

        var lines = rawInput.Split(Environment.NewLine);

        if (splitOn != null) lines = lines.SelectMany(l => l.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)).ToArray();

        return [.. lines];
    }

    public static char[][] GetLetterGrid(string input)
    {
        return GetInputLines(input)
            .Select(line => line.ToCharArray())
            .ToArray();
    }

    public static List<long> GetNumberLines(string input, char[] splitOn = null)
    {
        var rawInput = GetInputLines(input, splitOn);

        return rawInput
            .Where(ri => !string.IsNullOrWhiteSpace(ri))
            .Select(long.Parse)
            .ToList();
    }

    public static string[] GetBlocks(string input)
    {
        var rawInput = GetInput(input);
        return rawInput
            .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[][] GetBlockLines(string input)
    {
        var rawInput = GetInput(input);

        var blocks = rawInput
            .Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        return blocks
            .Select(block => block.Split(Environment.NewLine))
            .ToArray();
    }

    public static long[] GetNumbers(string input, params char[] splitOn)
    {
        var rawInput = GetInput(input);

        var split = rawInput.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);

        return split.Select(long.Parse).ToArray();
    }

    public static long[] GetDigits(string input)
    {
        var rawInput = GetInput(input);

        return rawInput.Select(c => c - 48L).ToArray();
    }

    public static long[][] GetDigitGrid(string input)
    {
        return GetInputLines(input)
            .Select(line => line.Select(c => c - 48L).ToArray())
            .ToArray();
    }
}