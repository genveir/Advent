namespace Advent2024.Day04;

public class Solution : ISolution
{
    public char[][] letters;

    public Solution(string input)
    {
        letters = Input.GetLetterGrid(input);
    }

    public Solution() : this("Input.txt")
    {
    }

    public long FindXMASCount()
    {
        long numXMAS = 0;

        for (int y = 0; y < letters.Length; y++)
        {
            for (int x = 0; x < letters[y].Length; x++)
            {
                numXMAS += FindXMAS(y, x);
            }
        }

        return numXMAS;
    }

    public long FindXMAS(int y, int x)
    {
        long numXMAS = 0;

        numXMAS += FindXMAS(y, x, -1, -1) ? 1 : 0;
        numXMAS += FindXMAS(y, x, -1, 0) ? 1 : 0;
        numXMAS += FindXMAS(y, x, -1, 1) ? 1 : 0;

        numXMAS += FindXMAS(y, x, 0, -1) ? 1 : 0;
        numXMAS += FindXMAS(y, x, 0, 1) ? 1 : 0;

        numXMAS += FindXMAS(y, x, 1, -1) ? 1 : 0;
        numXMAS += FindXMAS(y, x, 1, 0) ? 1 : 0;
        numXMAS += FindXMAS(y, x, 1, 1) ? 1 : 0;

        return numXMAS;
    }

    public bool FindXMAS(int y, int x, int dx, int dy)
    {
        return
            CheckLetter(y, x, 'X') &&
            CheckLetter(y + dy, x + dx, 'M') &&
            CheckLetter(y + 2 * dy, x + 2 * dx, 'A') &&
            CheckLetter(y + 3 * dy, x + 3 * dx, 'S');
    }

    public long FindMAS()
    {
        long numMAS = 0;

        for (int y = 0; y < letters.Length; y++)
        {
            for (int x = 0; x < letters[y].Length; x++)
            {
                if (FindMAS(y, x))
                {
                    numMAS++;
                }
            }
        }
        return numMAS;
    }

    public bool FindMAS(int y, int x)
    {
        if (CheckLetter(y, x, 'A'))
        {
            return CheckMASDiag(y, x);
        }

        return false;
    }

    private bool CheckMASDiag(int y, int x)
    {
        return
            (CheckLetter(y + 1, x + 1, 'M') && CheckLetter(y - 1, x - 1, 'S') ||
            CheckLetter(y - 1, x - 1, 'M') && CheckLetter(y + 1, x + 1, 'S')) &&
            (CheckLetter(y + 1, x - 1, 'M') && CheckLetter(y - 1, x + 1, 'S') ||
            CheckLetter(y - 1, x + 1, 'M') && CheckLetter(y + 1, x - 1, 'S'));
    }

    public bool CheckLetter(int y, int x, char letter)
    {
        if (y < 0) return false;
        if (y >= letters.Length) return false;
        if (x < 0) return false;
        if (x >= letters[y].Length) return false;

        return letters[y][x] == letter;
    }

    public object GetResult1()
    {
        return FindXMASCount();
    }

    public object GetResult2()
    {
        return FindMAS();
    }
}