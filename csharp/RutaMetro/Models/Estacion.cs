namespace CodeCamp.RutaMetro.Models
{
    using System.Collections.Generic;

    public class Estacion
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Coord Coord { get; set; }

        public IList<Linea> Lineas { get; set; } = new List<Linea>();

    }
}