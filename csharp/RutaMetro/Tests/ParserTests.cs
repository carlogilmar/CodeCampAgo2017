namespace CodeCamp.RutaMetro.Tests
{
    using System.Linq;

    using NUnit.Framework;

    using static CoordTests;

    [TestFixture]
    public class ParserTests
    {
        #region Constants

        // La ruta es relativa a partir del directorio "bin" del proyecto.
        private const string KlmPath = @"..\..\..\..\Metro_CDMX.kml.xml";

        #endregion

        #region Test Methods

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
        public void ParsesLineVertexes()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser.Lineas, Has.All.Property("Vertices").Not.Empty);

            var linea01 = parser.Lineas.First();
            Assert.That(linea01.Vertices.Count, Is.EqualTo(20));
            Assert.That(linea01.Vertices.First(), PointsAt(-99.2005488, 19.3982501));
            Assert.That(linea01.Vertices.Last(), PointsAt(-99.0722072, 19.4153591));

            var linea12 = parser.Lineas.Last();
            Assert.That(linea12.Vertices.Count, Is.EqualTo(20));
            Assert.That(linea12.Vertices.First(), PointsAt(-99.1878051, 19.3761645));
            Assert.That(linea12.Vertices.Last(), PointsAt(-99.0174150466919, 19.2906891806738));
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

            var zocalo = parser.Estaciones.Last();
            Assert.That(zocalo.Nombre, Is.EqualTo("Zócalo"));
            Assert.That(zocalo.Descripcion, Is.EqualTo("Línea 2. Centro, Cuauhtémoc, 06000 Ciudad de México, DF, México"));
        }

        [Test]
        public void ParsesStationCoordinates()
        {
            var parser = Parser.Parse(KlmPath);

            Assert.That(parser.Estaciones, Has.All.Property("Coord").Not.Null);
            Assert.That(parser.Estaciones.First().Coord, PointsAt(-99.0056777, 19.3647171));
            Assert.That(parser.Estaciones.Last().Coord, PointsAt(-99.1329861, 19.4332227));
        }

        #endregion
    }
}