using UnityEngine;

namespace GameAnalytics.Tooling
{
    public class Grid
    {
        public readonly int SizeX;
        public readonly int SizeY;
        public readonly int SizeZ;
        public readonly float Spacing;

        protected Grid(){}
        protected Grid(int sizeX, int sizeY, int sizeZ,float spacing)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Spacing = spacing;
        }
    }

    public class HeatMap : Grid
    {
        public readonly float[,,] Values;
        public Color MinColor = Color.blue;
        public Color MaxColor = Color.red;
        public HeatMap(int sizeX, int sizeY, int sizeZ, float spacing, float[,,] values)
            : base(sizeX, sizeY, sizeZ,spacing)
        {
            Values = values;
        }
    }
}
