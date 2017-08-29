namespace CodeCamp.RutaMetro
{
    using System.Linq;

    using CodeCamp.RutaMetro.Models;

    using NUnit.Framework;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Dsl;

    [TestFixture]
    public class MetroMapBuilderTests
    {
        #region Fields

        private static Fixture fixture;

        #endregion

        #region Test Methods

        [SetUp]
        public void Init()
        {
            fixture = new Fixture();
            fixture.Customize<Estacion>(o => o.Without(e => e.Lineas));
            fixture.Customize<Linea>(o => o.Without(e => e.Estaciones));
        }

        [Test]
        public void SinLineas()
        {
            var result = new MetroMapBuilder().Build();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Lineas, Is.Not.Null);
            Assert.That(result.Lineas, Is.Empty);
        }

        [Test]
        public void UnaEstacionSinLineas()
        {
            var estacion = MakeEstacion();

            var result = new MetroMapBuilder()
                .WithStations(new[] { estacion })
                .Build();

            Assert.That(result.Estaciones, Is.Not.Empty);
            Assert.That(result.Estaciones[0], Is.SameAs(estacion));
            Assert.That(result.Estaciones[0].Lineas, Is.Empty);
        }

        [Test]
        public void UnaLineaSinEstaciones()
        {
            var linea = MakeLinea();
            var lineas = new[] { linea };

            var result = new MetroMapBuilder()
                .WithLines(lineas)
                .Build();

            Assert.That(result.Lineas, Is.Not.Empty);
            Assert.That(result.Lineas[0], Is.SameAs(linea));
            Assert.That(result.Lineas[0].Estaciones, Is.Empty);
        }

        [Test]
        public void UnaLineaUnaEstacionSobreVertice()
        {
            var coord = fixture.Create<Coord>();
            var linea = MakeLinea(coord);
            var estacion = MakeEstacion(Clone(coord));

            var result = new MetroMapBuilder()
                .WithLines(new[] { linea })
                .WithStations(new[] { estacion })
                .Build();

            Assert.That(result.Lineas.First().Estaciones, Does.Contain(estacion),
                "La línea debería tener una estación");
            Assert.That(result.Estaciones.First().Lineas, Does.Contain(linea),
                "La estación debería pertenecer a una línea");
        }

        [Test]
        public void UnaLineaUnaEstacionLigeramenteFueraDelVertice()
        {
            var coordEstacion = new Coord(-99.1948807, 19.4905036);
            var estacion = MakeEstacion(coordEstacion);

            var coordLinea = coordEstacion.Offset(-0.0000000000001);
            var linea = MakeLinea(coordLinea);

            var result = new MetroMapBuilder()
                .WithLines(new[] { linea })
                .WithStations(new[] { estacion })
                .Build();

            Assert.That(result.Lineas.First().Estaciones, Does.Contain(estacion),
                "La línea debería tener una estación");
            Assert.That(result.Estaciones.First().Lineas, Does.Contain(linea),
                "La estación debería pertenecer a una línea");
        }

        #endregion

        #region Helper Methods

        private static Coord Clone(Coord coord)
        {
            return new Coord(coord.Latitud, coord.Longitud);
        }

        private static Estacion MakeEstacion(Coord? coord = null)
        {
            IPostprocessComposer<Estacion> builder = fixture.Build<Estacion>();

            if (coord != null)
            {
                builder = builder.With(e => e.Coord, coord.Value);
            }

            return builder.Create();
        }

        private static Linea MakeLinea(params Coord[] coords)
        {
            return fixture
                .Build<Linea>()
                .With(l => l.Vertices, coords)
                .Create();
        }

        #endregion
    }
}