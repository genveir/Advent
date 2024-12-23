namespace Advent2024.Day21;
public class Route
{
    public string InitialValue { get; set; }

    public const int LL = 0;
    public const int UU = 0;
    public const int DD = 0;
    public const int RR = 0;
    public const int AA = 0;

    public const int LU = 1;
    public const int LD = 2;
    public const int LA = 3;

    public const int UL = 4;
    public const int UR = 5;
    public const int UA = 6;

    public const int DL = 7;
    public const int DR = 8;
    public const int DA = 9;

    public const int RU = 10;
    public const int RD = 11;
    public const int RA = 12;

    public const int AL = 13;
    public const int AU = 14;
    public const int AD = 15;
    public const int AR = 16;

    public long[] Steps = new long[17];

    public long Length => Steps.Sum();

    public void Iterate()
    {
        var next = new long[17];

        // A -> "A"
        next[AA] += Steps[AA];

        // LU -> ">^A
        next[AR] += Steps[LU];
        next[RU] += Steps[LU];
        next[UA] += Steps[LU];

        // LD => ">A"
        next[AR] += Steps[LD];
        next[RA] += Steps[LD];

        // LA => ">>^A"
        next[AR] += Steps[LA];
        next[RR] += Steps[LA];
        next[RU] += Steps[LA];
        next[UA] += Steps[LA];

        // UL => "v<A"
        next[AD] += Steps[UL];
        next[DL] += Steps[UL];
        next[LA] += Steps[UL];

        // UR => "v>A"
        next[AD] += Steps[UR];
        next[DR] += Steps[UR];
        next[RA] += Steps[UR];

        // UA => ">A"
        next[AR] += Steps[UA];
        next[RA] += Steps[UA];

        // DL => "<A"
        next[AL] += Steps[DL];
        next[LA] += Steps[DL];

        // DR => ">A"
        next[AR] += Steps[DR];
        next[RA] += Steps[DR];

        // DA => "^>A"
        next[AU] += Steps[DA];
        next[UR] += Steps[DA];
        next[RA] += Steps[DA];

        // RU => "<^A"
        next[AL] += Steps[RU];
        next[LU] += Steps[RU];
        next[UA] += Steps[RU];

        // RD => "<A"
        next[AL] += Steps[RD];
        next[LA] += Steps[RD];

        // RA => "^A"
        next[AU] += Steps[RA];
        next[UA] += Steps[RA];

        // AL => "v<<A"
        next[AD] += Steps[AL];
        next[DL] += Steps[AL];
        next[LL] += Steps[AL];
        next[LA] += Steps[AL];

        // AU => "<A"
        next[AL] += Steps[AU];
        next[LA] += Steps[AU];

        // AD => "<vA"
        next[AL] += Steps[AD];
        next[LD] += Steps[AD];
        next[DA] += Steps[AD];

        // AR => "vA"
        next[AD] += Steps[AR];
        next[DA] += Steps[AR];

        Steps = next;
    }

    public static Route operator +(Route a, Route b)
    {
        var result = new Route()
        {
            InitialValue = a.InitialValue + b.InitialValue
        };

        for (int i = 0; i < result.Steps.Length; i++)
        {
            result.Steps[i] = a.Steps[i] + b.Steps[i];
        }

        return result;
    }

    public override int GetHashCode()
    {
        int result = 0;
        for (int i = 0; i < 21; i++)
        {
            result = (result * 397) ^ (int)Steps[i];
        }
        return result;
    }

    public override bool Equals(object obj)
    {
        if (obj is Route other)
        {
            for (int i = 0; i < Steps.Length; i++)
            {
                if (Steps[i] != other.Steps[i])
                    return false;
            }
            return true;
        }
        return false;
    }
}
