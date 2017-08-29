namespace CodeCamp.RutaMetro
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using CodeCamp.RutaMetro.Models;

    public class Parser
    {
        #region Public Properties

        public IList<Linea> Lineas { get; private set; }
        public IList<Estacion> Estaciones { get; private set; }

        #endregion

        #region Public Methods

        public static Parser Parse(string filePath)
        {
            var model = ReadModelFrom(filePath);
            var lineas = model.Document.Folders[0].Placemarks;
            var estaciones = model.Document.Folders[1].Placemarks;

            return new Parser
            {
                Lineas = lineas.Select(ToLinea).ToList(),
                Estaciones = estaciones.Select(ToEstacion).ToList()
            };
        }

        public static Kml ReadModelFrom(string filePath)
        {
            Kml result = null;

            using (var txtReader = new StreamReader(filePath))
            {
                var xmlSerializer = new XmlSerializer(typeof(Kml), Xmlns.Klm);
                result = (Kml)xmlSerializer.Deserialize(txtReader);
            }

            return result;
        }

        #endregion

        #region Methods

        private static Estacion ToEstacion(Placemark placemark)
        {
            return new Estacion
            {
                Nombre = placemark.Name,
                Descripcion = placemark.Description,
                Coord = ToCoords(placemark.Point.Coordinates).First()
            };
        }

        private static Coord ToCoord(string line)
        {
            //Console.WriteLine(line);
            var parts = line.Split(',')
                .Select(p => p.Trim())
                .Select(double.Parse)
                .ToArray();

            return new Coord(latitud: parts[0], longitud: parts[1]);
        }

        private static IList<Coord> ToCoords(string coordinates)
        {
            return coordinates
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(ToCoord)
                .ToList();
        }

        private static Linea ToLinea(Placemark placemark)
        {
            var coordinates = placemark.LineString.Coordinates;
            return new Linea
            {
                Nombre = placemark.Name,
                Vertices = ToCoords(coordinates)
            };
        }

        #endregion
    }
}