using System.Collections.Generic;
using System.Linq;
using Advent2023.Shared;

namespace Advent2023.Advent13;

public class Solution : ISolution
{
    public List<MirrorField> MirrorFields { get; set; } = new();

    public Solution(string input)
    {
        var blocks = Input.GetBlockLines(input);

        foreach (var block in blocks) MirrorFields.Add(new(block));
    }
    public Solution() : this("Input.txt") { }

    public class MirrorField
    {
        public char[][] MirrorLines;
        public char[][] PivotedMirrorLines;

        public MirrorField(string[] mirrorLines)
        {
            MirrorLines = mirrorLines.Select(ml => ml.ToCharArray()).ToArray();
            PivotedMirrorLines = MirrorLines.Pivot().Select(c => c.ToArray()).ToArray();
        }

        public int FindMirror(bool smudge)
        {
            return 100 * FindVertical(smudge) + FindHorizontal(smudge);
        }

        public int FindHorizontal(bool smudge) => FindMirror(PivotedMirrorLines, smudge);

        public int FindVertical(bool smudge) => FindMirror(MirrorLines, smudge);

        public int FindMirror(char[][] mirrors, bool smudge)
        {
            for (int n = 0; n < mirrors.Length - 1; n++)
            {
                if (CheckMirror(mirrors, n, smudge)) return n + 1;
            }
            return 0;
        }

        public bool CheckMirror(char[][] mirrors, int n, bool smudge) =>
            smudge ? CheckWithSmudgeNotFixed(mirrors, n, 0) : CheckWithSmudgeFixed(mirrors, n, 0);

        public bool CheckWithSmudgeNotFixed(char[][] mirrors, int n, int shift)
        {
            if (shift == 0 && CheckWithSmudgeFixed(mirrors, n, 0)) return false;

            var left = n - shift;
            var right = n + shift + 1;

            if (left < 0 || right > mirrors.Length - 1) return false;

            int differences = 0;
            for (int i = 0; i < mirrors[n].Length; i++)
            {
                if (mirrors[n - shift][i] != mirrors[n + shift + 1][i]) differences++;
            }

            return differences switch
            {
                0 => CheckWithSmudgeNotFixed(mirrors, n, shift + 1),
                1 => CheckWithSmudgeFixed(mirrors, n, shift + 1),
                _ => false
            };
        }

        public bool CheckWithSmudgeFixed(char[][] mirrors, int n, int shift)
        {
            while (true)
            {
                var left = n - shift;
                var right = n + shift + 1;

                if (left < 0 || right > mirrors.Length - 1) return true;

                if (!mirrors[left].SequenceEqual(mirrors[right])) return false;
                shift++;
            }
        }
    }

    public object GetResult1()
    {
        return MirrorFields.Sum(mf => mf.FindMirror(false));
    }

    public object GetResult2()
    {
        return MirrorFields.Sum(mf => mf.FindMirror(true));
    }
}
