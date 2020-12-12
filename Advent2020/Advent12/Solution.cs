using Advent2020.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent2020.Advent12
{
    public class Solution : ISolution
    {
        List<Instruction> instructions;

        public Solution(string input)
        {
            var lines = Input.GetInputLines(input).ToArray();

            instructions = lines.Select(line =>  Instruction.Parse(line.First(), int.Parse(line.Substring(1)))).ToList();
        }
        public Solution() : this("Input.txt") { }

        public abstract class Instruction
        {
            public int amount;

            public Instruction(int amount)
            {
                this.amount = amount;
            }

            public static Instruction Parse(char direction, int amount)
            {
                switch (direction)
                {
                    case 'N': return new North(amount);
                    case 'S': return new South(amount);
                    case 'E': return new East(amount);
                    case 'W': return new West(amount);
                    case 'L': return new Left(amount);
                    case 'R': return new Right(amount);
                    case 'F': return new Forward(amount);
                    default:
                        throw new NotImplementedException();
                }
            }

            public abstract void Execute(Ship ship);

            public abstract void Execute(Ship ship, WayPoint waypoint);
        }

        public class North : Instruction 
        { 
            public North(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.nsPos += amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.relNS += amount;
            }
        }
        public class South : Instruction 
        {
            public South(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.nsPos -= amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.relNS -= amount;
            }
        }
        public class East : Instruction 
        {
            public East(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.ewPos += amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.relEW += amount;
            }
        }
        public class West : Instruction 
        {
            public West(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.ewPos -= amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.relEW -= amount;
            }
        }
        public class Left : Instruction 
        {
            public Left(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.hdg -= amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.Rotate(-amount);
            }
        }
        public class Right : Instruction 
        {
            public Right(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.hdg += amount;
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                waypoint.Rotate(amount);
            }
        }

        public class Forward : Instruction 
        {
            public Forward(int amount) : base(amount) { }

            public override void Execute(Ship ship)
            {
                ship.hdg = ship.hdg % 360;
                ship.hdg += 360;
                ship.hdg = ship.hdg % 360;

                switch (ship.hdg)
                {
                    case 0: new East(amount).Execute(ship); break;
                    case 90: new South(amount).Execute(ship); break;
                    case 180: new West(amount).Execute(ship); break;
                    case 270: new North(amount).Execute(ship); break;
                    default:
                        throw new NotImplementedException();
                }
            }

            public override void Execute(Ship ship, WayPoint waypoint)
            {
                ship.nsPos += amount * waypoint.relNS;
                ship.ewPos += amount * waypoint.relEW;
            }
        }

        public class Ship
        {
            public int hdg = 0;
            public long nsPos = 0;
            public long ewPos = 0;
        }

        public class WayPoint
        {
            public int relNS = 1;
            public int relEW = 10;

            public void Rotate(int degrees)
            {
                degrees = degrees % 360;
                degrees += 360;
                degrees = degrees % 360;

                int curNS = relNS;
                int curEW = relEW;

                switch(degrees)
                {
                    case 0:     relNS =  curNS;     relEW =  curEW; return;
                    case 90:    relNS = -curEW;     relEW =  curNS; return;
                    case 180:   relNS = -curNS;     relEW = -curEW; return;
                    case 270:   relNS =  curEW;     relEW = -curNS; return;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public object GetResult1()
        {
            var ship = new Ship();

            foreach (var instruction in instructions) instruction.Execute(ship);

            return (int)(Math.Abs(ship.ewPos) + Math.Abs(ship.nsPos));
        }

        public object GetResult2()
        {
            var ship = new Ship();
            var waypoint = new WayPoint();

            foreach (var instruction in instructions) instruction.Execute(ship, waypoint);

            return (int)(Math.Abs(ship.ewPos) + Math.Abs(ship.nsPos));
        }
    }
}
