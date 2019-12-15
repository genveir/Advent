using System;
using System.Collections.Generic;
using System.Text;

namespace Advent2018.Advent13
{
    class TrackFactory
    {
        private Dictionary<XYCoord, TrainTrack> AllPositions = new Dictionary<XYCoord, TrainTrack>();

        public TrainTrack Parse(XYCoord coord, char input)
        {
            TrainTrack trackNorth;
            TrainTrack trackWest;
            AllPositions.TryGetValue(new XYCoord(coord.X, coord.Y - 1), out trackNorth);
            AllPositions.TryGetValue(new XYCoord(coord.X - 1, coord.Y), out trackWest);

            TrainTrack newTrack = null;
            switch (input)
            {
                case '|': newTrack = new NorthSouth(coord, trackNorth); break;
                case '\\': newTrack = new Corner(coord, trackNorth, trackWest); break;
                case '/': newTrack = new Corner(coord, trackNorth, trackWest); break;
                case '-': newTrack = new EastWest(coord, trackWest); break;
                case '+': newTrack = new Intersection(coord, trackNorth, trackWest); break;
            }

            AllPositions.Add(coord, newTrack);

            return newTrack;
        }
    }
}
