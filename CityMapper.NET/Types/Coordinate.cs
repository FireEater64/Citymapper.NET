namespace Citymapper.NET.Types
{
    public struct Coordinate
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public Coordinate(double givenLat, double givenLon)
        {
            Latitude = givenLat;
            Longitude = givenLon;
        }

        public override string ToString()
        {
            return string.Format($"{Latitude},{Longitude}");
        }
    }
}