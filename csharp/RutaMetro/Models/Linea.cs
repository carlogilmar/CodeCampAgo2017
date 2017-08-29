namespace CodeCamp.RutaMetro.Models
{
    using System.Collections.Generic;

    public class Linea
    {
        public string Nombre { get; set; }
        public IList<Coord> Vertices { get; set; }

        public IList<Estacion> Estaciones { get; set; } = new List<Estacion>();
    }
}