namespace CodeCamp.RutaMetro.Models
{
    using System;
    using static System.Math;

    public struct Coord : ICloneable
    {
        #region Constants

        private const double Epsilon = 0.0001;

        #endregion

        #region Fields

        private readonly double latitud;
        private readonly double longitud;

        #endregion

        #region Constructors & Destructors

        public Coord(double latitud, double longitud)
            : this()
        {
            this.latitud = latitud;
            this.longitud = longitud;
        }

        #endregion

        #region Public Properties

        public double Latitud => this.latitud;

        public double Longitud => this.longitud;

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

        public object Clone()
        {
            return new Coord(this.latitud, this.longitud);
        }

        public double Distance(Coord other)
        {
            return Sqrt(Pow(other.latitud - this.latitud, 2) + Pow(other.longitud - this.longitud, 2));
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
            result = result * -1521134295 + this.latitud.GetHashCode();
            result = result * -1521134295 + this.longitud.GetHashCode();
            return result;
        }

        public Coord Offset(double dLatitud = 0, double dLongitud = 0)
        {
            return new Coord(this.latitud + dLatitud, this.longitud + dLongitud);
        }

        public override string ToString()
        {
            return $"{GetType().Name}({latitud}, {longitud})";
        }

        #endregion
    }
}