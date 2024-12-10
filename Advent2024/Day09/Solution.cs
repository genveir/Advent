namespace Advent2024.Day09;

public class Solution
{
    public long[] Numbers { get; set; }
    public List<Block> Blocks { get; set; } = [];
    public List<Gap> Gaps { get; set; } = [];

    public Solution(string input)
    {
        Numbers = Input.GetDigits(input);

        Reset();
    }

    public void Reset()
    {
        Blocks.Clear();
        Gaps.Clear();

        long position = 0;
        long blockId = 0;
        for (int n = 0; n < Numbers.Length; n += 2)
        {
            var block = new Block(
                id: blockId++,
                firstPosition: position,
                length: Numbers[n]);
            Blocks.Add(block);

            position += Numbers[n];

            if (n < Numbers.Length - 1)
            {
                var gap = new Gap(
                    firstPosition: position,
                    length: Numbers[n + 1]);
                Gaps.Add(gap);

                position += Numbers[n + 1];
            }
        }
    }

    public Solution() : this("Input.txt")
    {
    }

    public class Block
    {
        public Block(long id, long firstPosition, long length)
        {
            Id = id;
            FirstPosition = firstPosition;
            Length = length;
        }

        public long Id { get; set; }

        public long FirstPosition { get; set; }

        public long Length { get; set; }

        public long Checksum()
        {
            long mult = FirstPosition;

            long sum = 0;
            for (int n = 0; n < Length; n++)
            {
                sum += mult * Id;
                mult++;
            }
            return sum;
        }

        public override string ToString()
        {
            return $"Block {Id} at {FirstPosition} with length {Length}";
        }
    }

    public class Gap
    {
        public Gap(long firstPosition, long length)
        {
            FirstPosition = firstPosition;
            Length = length;
        }

        public long FirstPosition { get; set; }
        public long Length { get; set; }

        public override string ToString()
        {
            return $"Gap at {FirstPosition} with length {Length}";
        }
    }

    public void Compress()
    {
        Reset();

        while (Gaps.Count > 0)
        {
            CompressStep();
        }

        Blocks = [.. Blocks.OrderBy(b => b.FirstPosition)];
    }

    public void CompressStep()
    {
        var lastBlock = Blocks.Last();

        var firstGap = Gaps.First();

        if (firstGap.Length >= lastBlock.Length)
        {
            lastBlock.FirstPosition = firstGap.FirstPosition;

            firstGap.Length -= lastBlock.Length;
            firstGap.FirstPosition += lastBlock.Length;

            if (firstGap.Length == 0)
            {
                Gaps.Remove(firstGap);
            }

            Blocks.Remove(lastBlock);
            Blocks.Insert(0, lastBlock);

            var lastGap = Gaps.Last();
            Gaps.Remove(lastGap);
        }
        else
        {
            Gaps.Remove(firstGap);

            if (firstGap.Length == 0)
            {
                return;
            }

            var newBlock = new Block(
                id: lastBlock.Id,
                firstPosition: firstGap.FirstPosition,
                length: firstGap.Length);

            Blocks.Insert(0, newBlock);

            lastBlock.Length -= firstGap.Length;
        }
    }

    public void Defragment()
    {
        Reset();

        var blocksToMove = Blocks.ToList();
        blocksToMove.Reverse();

        for (int n = 0; n < blocksToMove.Count; n++)
        {
            DefragmentStep(blocksToMove[n]);
        }

        Blocks = [.. Blocks.OrderBy(b => b.FirstPosition)];
    }

    public void DefragmentStep(Block blockToMove)
    {
        var firstGap = Gaps
            .FirstOrDefault(g => g.Length >= blockToMove.Length && g.FirstPosition < blockToMove.FirstPosition);

        if (firstGap == null)
        {
            return;
        }

        blockToMove.FirstPosition = firstGap.FirstPosition;
        firstGap.FirstPosition += blockToMove.Length;
        firstGap.Length -= blockToMove.Length;
    }

    public long CheckSum()
    {
        return Blocks.Sum(b => b.Checksum());
    }

    // not 85836859273
    public object GetResult1()
    {
        Compress();

        return CheckSum();
    }

    // 8532024062389 too high
    public object GetResult2()
    {
        Defragment();

        return CheckSum();
    }
}