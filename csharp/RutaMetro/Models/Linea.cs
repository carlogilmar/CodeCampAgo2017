namespace CodeCamp.RutaMetro.Models
{
    using System.Collections.Generic;

    public class Linea
    {
        public IList<Coord> Vertices { get; set; }
        public string Nombre { get; set; }
    }
}