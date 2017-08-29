namespace CodeCamp.RutaMetro.Models
{
    using static System.Math;

    public struct Coord
    {
        #region Constants

        private const double Epsilon = 0.00001;

        #endregion

        #region Constructors & Destructors

        public Coord(double latitud, double longitud)
            : this()
        {
            this.Latitud = latitud;
            this.Longitud = longitud;
        }

        #endregion

        #region Public Properties

        public double Latitud { get; private set; }

        public double Longitud { get; private set; }

        #endregion

        #region Public Methods

        public static bool operator !=(Coord coord1, Coord coord2)
        {
            return !(coord1 == coord2);
        }

        public static bool operator ==(Coord coord1, Coord coord2)
        {
            return coord1.Equals(coord2);
        }

        public double Distance(Coord other)
        {
            return Sqrt(Pow(other.Latitud - this.Latitud, 2) + Pow(other.Longitud - this.Longitud, 2));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Coord))
            {
                return base.Equals(obj);
            }

            var other = (Coord)obj;
            return this.Distance(other) <= Epsilon;
        }

        public override int GetHashCode()
        {
            var result = base.GetHashCode();
            result = result * -1521134295 + this.Latitud.GetHashCode();
            result = result * -1521134295 + this.Longitud.GetHashCode();
            return result;
        }

        public Coord Offset(double dLatitud = 0, double dLongitud = 0)
        {
            return new Coord(this.Latitud + dLatitud, this.Longitud + dLongitud);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Latitud}m, {Longitud}m)";
        }

        #endregion
    }
}