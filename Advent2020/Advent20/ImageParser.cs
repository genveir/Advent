using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2020.Advent20
{
    public class ImageParser
    {
        /*
                          # 
        #    ##    ##    ###
         #  #  #  #  #  #    
        01234567890123456789
        */

        public long Solve(PuzzlePiece fusedPuzzle)
        {
            string[] stringData = new string[0];
            bool[][] matchData = new bool[0][];

            bool orientationIsCorrect = false;
            for (int rot = 0; rot < 8; rot++)
            {
                fusedPuzzle.Rotation = rot;
                stringData = fusedPuzzle.GetStringData();
                matchData = new bool[stringData.Length][];
                for (int n = 0; n < matchData.Length; n++) matchData[n] = new bool[stringData[n].Length];

                for (int y = 0; y < stringData.Length; y++)
                {
                    for (int x = 0; x < stringData[y].Length; x++)
                    {
                        if (matchData[y][x]) continue;

                        if (stringData[y][x] == '#')
                        {
                            var isMatch = CheckMatch(stringData, x, y);

                            if (isMatch)
                            {
                                SetMatch(matchData, x, y);
                                orientationIsCorrect = true;
                            }
                        }
                    }
                }
                if (orientationIsCorrect) break;
            }

            int roughness = 0;
            for (int y = 0; y < stringData.Length; y++)
            {
                for (int x = 0; x < stringData[y].Length; x++)
                {
                    if (stringData[y][x] == '#' && !matchData[y][x]) roughness++;
                }
            }

            return roughness;
        }

        /*
        87654321098765432101
                          # 
        #    ##    ##    ###
         #  #  #  #  #  #    
        01234567890123456789
        */

        private bool CheckMatch(string[] stringData, int x, int y)
        {
            if (!CheckSpot(stringData, y + 1, x + 1 )) return false;
            if (!CheckSpot(stringData, y + 1, x     )) return false;
            if (!CheckSpot(stringData, y + 1, x - 1 )) return false;
            if (!CheckSpot(stringData, y + 2, x - 2 )) return false;
            if (!CheckSpot(stringData, y + 2, x - 2 )) return false;

            if (!CheckSpot(stringData, y + 2, x - 5 )) return false;
            if (!CheckSpot(stringData, y + 1, x - 6 )) return false;
            if (!CheckSpot(stringData, y + 1, x - 7 )) return false;
            if (!CheckSpot(stringData, y + 2, x - 8 )) return false;

            if (!CheckSpot(stringData, y + 2, x - 11)) return false;
            if (!CheckSpot(stringData, y + 1, x - 12)) return false;
            if (!CheckSpot(stringData, y + 1, x - 13)) return false;
            if (!CheckSpot(stringData, y + 2, x - 14)) return false;

            if (!CheckSpot(stringData, y + 2, x - 17)) return false;
            if (!CheckSpot(stringData, y + 1, x - 18)) return false;

            return true;
        }

        private bool CheckSpot(string[] stringData, int y, int x)
        {
            if (x < 0) return false;
            if (y < 0) return false;
            if (y >= stringData.Length) return false;
            if (x >= stringData[y].Length) return false;

            return (stringData[y][x] == '#');
        }

        private void SetMatch(bool[][] matchData, int x, int y)
        {
            matchData[y    ][x     ] = true;

            matchData[y + 1][x + 1 ] = true;
            matchData[y + 1][x     ] = true;
            matchData[y + 1][x - 1 ] = true;
            matchData[y + 2][x - 2 ] = true;
            matchData[y + 2][x - 2 ] = true;

            matchData[y + 2][x - 5 ] = true;
            matchData[y + 1][x - 6 ] = true;
            matchData[y + 1][x - 7 ] = true;
            matchData[y + 2][x - 8 ] = true;

            matchData[y + 2][x - 11] = true;
            matchData[y + 1][x - 12] = true;
            matchData[y + 1][x - 13] = true;
            matchData[y + 2][x - 14] = true;

            matchData[y + 2][x - 17] = true;
            matchData[y + 1][x - 18] = true;
        }
    }
}
