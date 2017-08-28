namespace CodeCamp.RutaMetro
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using NUnit.Framework;

    [TestFixture]
    public class ParserTests
    {
        // La ruta es relativa a partir del directorio "bin" del proyecto.
        private const string KlmPath = @"..\..\..\..\Metro_CDMX.kml.xml";

        [Test]
        public void ReadModelFrom()
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
        public void ParseLines()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser, Is.Not.Null);
            Assert.That(parser.Lineas.Count, Is.EqualTo(12));
            Assert.That(parser.Lineas.First().Name, Is.EqualTo("Línea 1"));
            Assert.That(parser.Lineas.Last().Name, Is.EqualTo("Línea 12"));
        }

        [Test]
        public void ParseLineCoordinates()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser.Lineas, Has.All.Property("Coords").Not.Null);
        }
    }

    public class Linea
    {
        public string Name { get; set; }
        public IList<Coord> Coords { get; set; }
    }

    public class Coord
    {
    }

    public class Parser
    {
        public IList<Linea> Lineas { get; private set; }

        public static Parser Parse(string filePath)
        {
            var model = ReadModelFrom(filePath);
            var lineas = model.Document.Folders[0].Placemarks;

            return new Parser
            {
                Lineas = lineas.Select(p => ToLinea(p)).ToList()
            };
        }

        private static Linea ToLinea(Placemark placemark)
        {
            return new Linea
            {
                Name = placemark.Name,
                Coords = new Coord[0]
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
    }
}