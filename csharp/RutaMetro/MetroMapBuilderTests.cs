namespace CodeCamp.RutaMetro
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CodeCamp.RutaMetro.Models;

    using NUnit.Framework;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Dsl;

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
                    if (linea.Vertices.Any(v => estacion.Coord.Equals(v)))
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

    [TestFixture]
    public class MetroMapBuilderTests
    {
        #region Fields

        private static Fixture fixture;

        #endregion

        #region Public Methods

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

            Assert.That(result.Lineas, Is.Not.Empty);
            Assert.That(result.Lineas.First().Estaciones, Is.Not.Empty);
            Assert.That(result.Estaciones, Is.Not.Empty);
            Assert.That(result.Estaciones.First().Lineas, Is.Not.Empty);
        }

        private static Coord Clone(Coord coord)
        {
            return new Coord(coord.Latitud, coord.Longitud);
        }

        #endregion

        #region Methods

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