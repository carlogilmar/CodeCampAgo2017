namespace CodeCamp.RutaMetro.Models
{
    public struct Coord
    {
        #region Constructors & Destructors

        public Coord(decimal latitud, decimal longitud) : this()
        {
            this.Latitud = latitud;
            this.Longitud = longitud;
        }

        #endregion

        #region Public Properties

        public decimal Latitud { get; private set; }
        public decimal Longitud { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"{GetType().Name}({Latitud}m, {Longitud}m)";
        }

        #endregion
    }
}