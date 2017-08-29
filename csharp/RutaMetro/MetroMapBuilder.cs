namespace CodeCamp.RutaMetro
{
    using System.Collections.Generic;
    using System.Linq;

    using CodeCamp.RutaMetro.Models;
    public class MetroMapBuilder
    {
        #region Fields

        private IEnumerable<Linea> lines = Enumerable.Empty<Linea>();

        public IEnumerable<Estacion> stations = Enumerable.Empty<Estacion>();

        #endregion

        #region Public Methods

        public Mapa Build()
        {
            var mapa = new Mapa
            {
                Lineas = this.lines.ToList(),
                Estaciones = this.stations.ToList(),
            };

            foreach(var estacion in mapa.Estaciones)
            {
                foreach(var linea in mapa.Lineas)
                {
                    if (linea.Vertices.Any(v => estacion.Coord == v))
                    {
                        linea.Estaciones.Add(estacion);
                        estacion.Lineas.Add(linea);
                    }
                }
            }

            return mapa;
        }

        public MetroMapBuilder WithLines(IEnumerable<Linea> moreLines)
        {
            this.lines = this.lines.Concat(moreLines);
            return this;
        }

        public MetroMapBuilder WithStations(IEnumerable<Estacion> moreStations)
        {
            this.stations = this.stations.Concat(moreStations);
            return this;
        }

        #endregion

        public class Mapa
        {
            public IList<Linea> Lineas { get; set; }

            public IList<Estacion> Estaciones { get; set; }
        }
    }
}