namespace CodeCamp.RutaMetro
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    public class Parser
    {
        public IList<Linea> Lineas { get; private set; }
        public IList<Estacion> Estaciones { get; private set; }

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

        private static Estacion ToEstacion(Placemark placemark)
        {
            return new Estacion
            {
                Nombre = placemark.Name,
                Descripcion = placemark.Description,
                Coord = ToCoords(placemark.Point.Coordinates).First()
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

        private static Coord ToCoord(string line)
        {
            //Console.WriteLine(line);
            var parts = line.Split(',')
                .Select(p => p.Trim())
                .Select(decimal.Parse)
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
    }
}