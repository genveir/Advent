using Advent2022.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent03
{
    public class FastSolution : ISolution
    {
        int elf;
        readonly int[] data;
        readonly int[] identity;

        public FastSolution(string input)
        {
            var lines = Input.GetInputLines(input).Select(i => i.ToCharArray()).ToArray();
            identity = Enumerable.Range(0, 53).ToArray();

            data = new int[lines.Length * 53];

            SetData(lines);
        }

        private void SetData(char[][] lines)
        {
            var length = lines.Length;

            var hundreds = length / 100;
            var tens = length % 100 / 10;
            var ones = length % 10;

            for (int n = 0; n < hundreds; n++) Set100Lines(lines, n);
            for (int n = 0; n < tens; n++) Set10Lines(lines, hundreds * 100 + n * 10);
            for (int n = 0; n < ones; n++) SetLine(lines[hundreds* 100 + tens * 10 + n]);
        }

        private void Set100Lines(char[][] lines, int hundredsIndex)
        {
            var lineIndex = 100 * hundredsIndex;

            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex); lineIndex += 10;
            Set10Lines(lines, lineIndex);
        }

        private void Set10Lines(char[][] lines, int lineIndex)
        {
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]); 
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex++]);
            SetLine(lines[lineIndex]);
        }

        private void SetLine(char[] line)
        {
            var tens = line.Length / 10;
            var ones = line.Length % 10;

            for (int n = 0; n < tens; n++) Set10Chars(line, n);
            for (int n = 0; n < ones; n++) SetChar(line, tens * 10 + n);

            elf++;
        }

        private void Set10Chars(char[] line, int tensIndex)
        {
            var lineIndex = tensIndex * 10;

            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex++);
            SetChar(line, lineIndex);
        }

        private void SetChar(char[] line, int lineIndex) 
            => SetChar(line[lineIndex]);

        private void SetChar(char active)
        {
            var priority = active > 96 ? active - 96 : active - 38;

            data[elf * 53 + priority] = 1;
        }

        public object GetResult1()
        {
            return "";
        }

        public object GetResult2()
        {
            var numGroups = data.Length / 159;

            var tens = numGroups / 10;
            var ones = numGroups % 10;

            var result = 0;
            for (int n = 0; n < tens; n++) result += Get10BadgePriorities(n);
            for (int n = 0; n < ones; n++) result += GetBadgePriority(n);

            return result;
        }

        private int Get10BadgePriorities(int tensIndex)
        {
            var groupIndex = tensIndex * 10;
            var result = 0;

            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex++);
            result += GetBadgePriority(groupIndex);

            return result;
        }

        private int GetBadgePriority(int groupIndex)
        {
            return
                Get10RowPriorities(groupIndex, 0) +
                Get10RowPriorities(groupIndex, 1) +
                Get10RowPriorities(groupIndex, 2) +
                Get10RowPriorities(groupIndex, 3) +
                Get10RowPriorities(groupIndex, 4) +
                GetRowPriority(groupIndex, 50) +
                GetRowPriority(groupIndex, 51) +
                GetRowPriority(groupIndex, 52);
        }

        private int Get10RowPriorities(int groupIndex, int tens)
        {
            var rowIndex = tens * 10;

            return 
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex++) +
                GetRowPriority(groupIndex, rowIndex);
        }

        private int GetRowPriority(int groupIndex, int row) =>
            data[groupIndex * 159 + row] * 
            data[groupIndex * 159 + 53 + row] * 
            data[groupIndex * 159 + 106 + row] * 
            identity[row];
    }
}
