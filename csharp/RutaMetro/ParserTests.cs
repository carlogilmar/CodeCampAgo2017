namespace CodeCamp.RutaMetro
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    [TestFixture]
    public class ParserTests
    {
        // La ruta es relativa a partir del directorio "bin" del proyecto.
        private const string KlmPath = @"..\..\..\..\Metro_CDMX.kml.xml";

        [Test]
        public void ReadsModelFromFile()
        {
            var model = Parser.ReadModelFrom(KlmPath);

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Document.Folders.Count, Is.EqualTo(3));

            var lineas = model.Document.Folders[0];
            Assert.That(lineas.Name, Is.EqualTo("Líneas de Metro"));
            Assert.That(lineas.Placemarks.Count, Is.EqualTo(12));

            var estaciones = model.Document.Folders[1];
            Assert.That(estaciones.Name, Is.EqualTo("Estaciones de Metro"));
            Assert.That(estaciones.Placemarks.Count, Is.EqualTo(162));
        }

        [Test]
        public void ParsesLines()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser, Is.Not.Null);
            Assert.That(parser.Lineas.Count, Is.EqualTo(12));
            Assert.That(parser.Lineas.First().Nombre, Is.EqualTo("Línea 1"));
            Assert.That(parser.Lineas.Last().Nombre, Is.EqualTo("Línea 12"));
        }

        [Test]
        public void ParsesLineCoordinates()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser.Lineas, Has.All.Property("Coords").Not.Empty);

            var linea01 = parser.Lineas.First();
            Assert.That(linea01.Coords.Count, Is.EqualTo(20));
            Assert.That(linea01.Coords.First(), PointsAt(-99.2005488m, 19.3982501m));
            Assert.That(linea01.Coords.Last(), PointsAt(-99.0722072m, 19.4153591m));

            var linea12 = parser.Lineas.Last();
            Assert.That(linea12.Coords.Count, Is.EqualTo(20));
            Assert.That(linea12.Coords.First(), PointsAt(-99.1878051m, 19.3761645m));
            Assert.That(linea12.Coords.Last(), PointsAt(-99.0174150466919m, 19.2906891806738m));
        }

        [Test]
        public void ParsesStations()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser, Is.Not.Null);
            Assert.That(parser.Estaciones.Count, Is.EqualTo(162));

            var acatitla = parser.Estaciones.First();
            Assert.That(acatitla.Nombre, Is.EqualTo("Acatitla"));
            Assert.That(acatitla.Descripcion, Is.EqualTo("Línea A. Santa Martha Acatitla, Iztapalapa, Ciudad de México, DF, México"));

            var acatitla2 = parser.Estaciones.Last();
            Assert.That(acatitla2.Nombre, Is.EqualTo("Zócalo"));
            Assert.That(acatitla2.Descripcion, Is.EqualTo("Línea 2. Centro, Cuauhtémoc, 06000 Ciudad de México, DF, México"));
        }

        private static EqualConstraint PointsAt(decimal latitud, decimal longitud)
        {
            return Is.EqualTo(new Coord(latitud, longitud)).Using<Coord>(AreEqual);
        }

        private static int AreEqual(Coord self, Coord other)
        {
            return self.Latitud == other.Latitud && self.Longitud == other.Longitud
                ? 0 : 1;
        }
    }

    public class Linea
    {
        public IList<Coord> Coords { get; set; }
        public string Nombre { get; set; }
    }

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

    public class Estacion
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

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
                Descripcion = placemark.Description
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
                Coords = ToCoords(coordinates)
            };
        }
    }
}