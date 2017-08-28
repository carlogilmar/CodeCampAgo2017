namespace CodeCamp.RutaMetro.Models
{
    public class Coord
    {
        public Coord(decimal latitud, decimal longitud)
        {
            this.Latitud = latitud;
            this.Longitud = longitud;
        }

        public decimal Latitud { get; private set; }
        public decimal Longitud { get; private set; }

        public override string ToString()
        {
            return $"{GetType().Name}({Latitud}m, {Longitud}m)";
        }
    }
}