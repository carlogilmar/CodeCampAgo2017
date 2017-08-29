namespace CodeCamp.RutaMetro.Tests
{
    using CodeCamp.RutaMetro.Models;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    using Ploeh.AutoFixture.NUnit3;

    using static System.Math;

    [TestFixture]
    public class CoordTests
    {
        #region Test Methods

        [Test, AutoData]
        public void Clone(Coord c1)
        {
            var c2 = (Coord)c1.Clone();

            Assert.That(c1, Is.Not.SameAs(c2));
            Assert.That(c1.Latitud, Is.EqualTo(c2.Latitud), nameof(c1.Latitud));
            Assert.That(c1.Longitud, Is.EqualTo(c2.Longitud), nameof(c1.Longitud));
        }

        [Test, AutoData]
        public void Distance_Ambos(Coord c1, double dLatitud, double dLongitud)
        {
            var c2 = c1.Offset(dLatitud, dLongitud);

            var result = c2.Distance(c1);

            Assert.That(result, Is.GreaterThan(Abs(dLatitud)),  "Debe ser mayor a la magnitud del delta de latitud");
            Assert.That(result, Is.GreaterThan(Abs(dLongitud)), "Debe ser mayor a la magnitud del delta de longitud");
            Assert.That(result, Is.LessThan(Abs(dLatitud) + Abs(dLongitud)), "Debe ser menor a la suma de magnitudes");
            Assert.That(result, Is.EqualTo(c1.Distance(c2)), "debe ser conmutativa");
        }

        [Test, AutoData]
        public void Distance_SoloDiferenteLatitud(Coord c1, double delta)
        {
            var c2 = c1.Offset(dLatitud: delta);

            var result = c2.Distance(c1);

            Assert.That(result, Is.EqualTo(delta), "Magnitud");
            Assert.That(result, Is.EqualTo(c1.Distance(c2)), "debe ser conmutativa");
        }

        [Test, AutoData]
        public void Distance_SoloDiferenteLongitud(Coord c1, double delta)
        {
            var c2 = c1.Offset(dLongitud: delta);

            var result = c2.Distance(c1);

            Assert.That(result, Is.EqualTo(delta), "Magnitud");
            Assert.That(result, Is.EqualTo(c1.Distance(c2)), "debe ser conmutativa");
        }

        [Test, AutoData]
        public void Equals_DentroDeTolerancia(Coord c1)
        {
            var c2 = c1.Offset(0.00001, 0.00001);

            Assert.That(c1, Is.EqualTo(c2), "Is.EqualTo");
            Assert.IsTrue(c1.Equals(c2), "Equals method");
            Assert.IsTrue(c1 == c2, "operator==");
            Assert.IsFalse(c1 != c2, "operator!=");
        }

        [Test, AutoData]
        public void Equals_FueraDeTolerancia(Coord c1)
        {
            var c2 = c1.Offset(0.001, 0.001);

            Assert.That(c1, Is.Not.EqualTo(c2), "Is.Not.EqualTo");
            Assert.IsFalse(c1.Equals(c2), "Equals method");
            Assert.IsFalse(c1 == c2, "operator==");
            Assert.IsTrue(c1 != c2, "operator!=");
        }

        [Test, AutoData]
        public void Equals_HappyPath(Coord c1)
        {
            var c2 = (Coord)c1.Clone();

            Assert.That(c1, Is.EqualTo(c2), "Is.EqualTo");
            Assert.IsTrue(c1.Equals(c2), "Equals method");
            Assert.IsTrue(c1 == c2, "operator==");
            Assert.IsFalse(c1 != c2, "operator!=");
        }

        [Test, AutoData]
        public void GetHashCode_FueraDeTolerancia(Coord c1)
        {
            var c2 = c1.Offset(0.001, 0.001);

            Assert.That(c1.GetHashCode(), Is.Not.EqualTo(c2.GetHashCode()));
        }

        [Test, AutoData]
        public void GetHashCode_HappyPath(Coord c1)
        {
            var c2 = ((Coord)c1.Clone()).GetHashCode();

            Assert.That(c1.GetHashCode(), Is.EqualTo(c2.GetHashCode()));
        }

        [Test, AutoData]
        public void Offset(Coord c1, double latitud, double longitud)
        {
            var c2 = c1.Offset(latitud, longitud);

            Assert.That(c2.Latitud, Is.EqualTo(c1.Latitud + latitud), nameof(c1.Latitud));
            Assert.That(c2.Longitud, Is.EqualTo(c1.Longitud + longitud), nameof(c1.Longitud));
        }

        [Test, AutoData]
        public void Offset_SoloLatitud(Coord c1, double latitud)
        {
            var c2 = c1.Offset(latitud);

            Assert.That(c2.Latitud, Is.EqualTo(c1.Latitud + latitud), nameof(c1.Latitud));
            Assert.That(c2.Longitud, Is.EqualTo(c1.Longitud), nameof(c1.Longitud));
        }

        [Test, AutoData]
        public void Offset_SoloLongitud(Coord c1, double longitud)
        {
            var c2 = c1.Offset(dLongitud: longitud);

            Assert.That(c2.Latitud, Is.EqualTo(c1.Latitud), nameof(c1.Latitud));
            Assert.That(c2.Longitud, Is.EqualTo(c1.Longitud + longitud), nameof(c1.Longitud));
        }

        #endregion

        #region Helper Methods

        internal static EqualConstraint PointsAt(double latitud, double longitud)
        {
            return Is.EqualTo(new Coord(latitud, longitud));
        }

        #endregion
    }
}