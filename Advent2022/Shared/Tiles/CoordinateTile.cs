﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2022.Shared.Tiles
{
    public class CoordinateTile<TValue> : BaseTile<CoordinateTile<TValue>>
    {
        public Coordinate Coordinate { get; set; }
        public TValue Value { get; set; }

        public CoordinateTile(Coordinate coordinate, TValue value)
        {
            Coordinate = coordinate;
            Value = value;
        }
    }
}
