using Advent2022.ElfFileSystem;
using Advent2022.Shared;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Advent15
{
    public class Solution : ISolution
    {
        public List<Sensor> sensors;
        public HashSet<Coordinate> beacons;
        public long Row = 2_000_000;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            var inputParser = new InputParser(false, 4, "Sensor at x=", ", y=", ": closest beacon is at x=", ", y=");

            sensors = inputParser.Parse<Sensor>(lines);

            beacons = new(sensors.Select(s => s.ClosestBeacon));
        }
        public Solution() : this("Input.txt") { }

        public class Sensor
        {
            public Coordinate Position;
            public Coordinate ClosestBeacon;
            public long Distance => Position.ManhattanDistance(ClosestBeacon);

            [ComplexParserConstructor]
            public Sensor(long sensorX, long sensorY, long beaconX, long beaconY)
            {
                Position = new(sensorX, sensorY);
                ClosestBeacon = new(beaconX, beaconY);
            }

            public RowSegment GetSegmentOnRow(long row)
            {
                var rowDiff = Math.Abs(Position.Y - row);
                var shift = Distance - rowDiff;

                if (shift < 0) return new(false, row, 0, 0);

                return new(true, row, Position.X - shift, Position.X + shift);
            }
        }

        public struct RowSegment
        {
            public bool HasValue;
            public long Row;
            public long Left;
            public long Right;
            public long Size => Right - Left + 1;

            public RowSegment(bool hasBounds, long row, long left, long right)
            {
                HasValue = hasBounds;
                Row = row;
                Left = left;
                Right = right;
            }

            public static bool TryMergeSegments(RowSegment left, RowSegment right, out RowSegment result)
            {
                result = left;

                if (!(left.HasValue || right.HasValue)) return false;
                if (left.Row != right.Row) return false;
                
                if (left.Left <= right.Left && left.Right >= right.Left)
                {
                    result = new(true, left.Row, left.Left, Math.Max(left.Right, right.Right));
                    return true;
                }

                if (left.Right >= right.Right && left.Left <= right.Right)
                {
                    result = new(true, left.Row, Math.Min(left.Left, right.Left), left.Right);
                    return true;
                }

                return false;
            }

            public RowSegment Trim()
            {
                long left = Left < 0 ? 0 : Left;
                long right = Right > 4_000_000 ? 4_000_000 : Right;

                return new(true, Row, left, right);
            }

            public override int GetHashCode()
            {
                return (int)Left * 7 * (int)Right + 12798 * (int)Row;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                var other = (RowSegment)obj;

                return other.HasValue == this.HasValue &&
                    other.Row == this.Row &&
                    other.Left == this.Left &&
                    other.Right == this.Right;
            }

            public override string ToString()
            {
                var leftRight = $"{Left} - {Right}";
                var noValue = "Empty";

                return $"Segment of row {Row} {(HasValue ? leftRight : noValue)}";
            }
        }

        public class RowPartial
        {
            public long Row;
            public List<RowSegment> Segments;
            public long Size => Segments.Sum(rb => rb.Size);

            public RowPartial(long row)
            {
                Row = row;
                Segments = new();
            }

            public static RowPartial FromSegments(IEnumerable<RowSegment> segments, long row)
            {
                var ordered = segments
                    .Where(seg => seg.HasValue)
                    .Where(seg => seg.Row == row)
                    .OrderBy(seg => seg.Left)
                    .ToArray();

                var partial = new RowPartial(row);
                foreach(var segment in ordered)
                {
                    partial.AddOrderedSegment(segment);
                }
                return partial;
            }

            int index = 0;
            public void AddOrderedSegment(RowSegment segment)
            {
                bool added = false;
                while (!added)
                {
                    if (index == Segments.Count)
                    {
                        Segments.Add(segment);
                        added = true;
                    }
                    else
                    {
                        var curSegment = Segments[index];
                        if (RowSegment.TryMergeSegments(curSegment, segment, out RowSegment result))
                        {
                            Segments[index] = result;
                            added = true;
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
            }

            public void Trim()
            {
                List<RowSegment> newSegments = new();
                foreach(var segment in Segments)
                {
                    var newSegment = new RowSegment(segment.HasValue, segment.Row, segment.Left, segment.Right);

                    if (newSegment.Right < 0 || newSegment.Left > 4_000_000) continue;

                    newSegments.Add(newSegment.Trim());
                }
                Segments = newSegments;
            }
        }

        public long NumberThatCannotBeABeaconOnRow(long row)
        {
            var segments = sensors
                .Select(s => s.GetSegmentOnRow(row));

            var partial = RowPartial.FromSegments(segments, row);

            return partial.Size - beacons.Count(coord => coord.Y == row);
        }

        public long FindMissingBeacon()
        {
            long result = 0;
            Parallel.For(0, 4_000_000, (row, state) =>
            {
                var segments = sensors
                    .Select(s => s.GetSegmentOnRow(row));

                var partial = RowPartial.FromSegments(segments, row);
                partial.Trim();

                if (partial.Segments.Count > 1)
                {
                    var y = row;
                    var x = partial.Segments[0].Right + 1;

                    result = x * 4_000_000 + y;
                    state.Stop();
                }
            });

            return result;
        }

        public object GetResult1()
        {
            return NumberThatCannotBeABeaconOnRow(Row);
        }

        public object GetResult2()
        {
            return FindMissingBeacon();
        }
    }
}
