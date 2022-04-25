using FluentAssertions;
using SchnauzEngine;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SchnauzEngineTest
{
    public class StapelTest
    {
        private readonly ITestOutputHelper output;
        private readonly Stapel stapel;

        public StapelTest(ITestOutputHelper output)
        {
            this.output = output;
            this.stapel = new Stapel();
        }

        [Fact]
        public void Stapel_hat_32_Karten()
        {
            
            stapel.Karten.Should().HaveCount(32);

            // Equals / Hashcode unique
            stapel.Karten.Should().OnlyHaveUniqueItems();

            var kartenNamen = stapel.Karten.Select(k => k.ToString()).ToList();
            kartenNamen.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Alle_Karten_sind_unterschiedlich()
        {
            stapel.Karten.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Alle_Karten_haben_unterschiedliche_Texte()
        {
            var kartenNamen = stapel.Karten.Select(k => k.ToString()).ToList();
            kartenNamen.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public void Neuer_Stapel_Gleich_viele_Karten()
        {
            Stapel erstesMischen = new Stapel();

            erstesMischen.Karten.Should().HaveSameCount(stapel.Karten);
        }

        [Fact]
        public void Neuer_Stapel_Gleiche_Karten()
        {
            Stapel erstesMischen = new Stapel();

            erstesMischen.Karten.Should().Contain(stapel.Karten);
            erstesMischen.Karten.Should().BeEquivalentTo(stapel.Karten);
        }

        [Fact]
        public void Neuer_Stapel_Andere_Reihenfolge()
        {
            Stapel erstesMischen = new Stapel();

            // This could fail quite often, as sometimes there first card might not be in different order
            erstesMischen.Karten.First().Should().NotBe(stapel.Karten.First());
            //TODO: Assert different
        }

        [Fact]
        public void Ziehen_Anzahl_reduziert()
        {
            (var nachher, var gezogen) = stapel.DreiZiehen();
            nachher.Karten.Should().HaveCount(29);
            gezogen.Karten.Should().HaveCount(3);
        }

        [Fact]
        public void Ziehen_Karten_nicht_mehr_im_Stapel()
        {
            (var nachher, var gezogen) = stapel.DreiZiehen();
            nachher.Karten.Should().NotContain(gezogen.Karten);
            nachher.Karten.Except(gezogen.Karten).Should().HaveSameCount(nachher.Karten);
        }

        [Fact]
        public void Ziehen_StapelLeet()
        {
            var nachher = stapel;
            for (int i = 0; i < stapel.Karten.Count / 3; i++) { 
                (nachher, _) = nachher.DreiZiehen();
            }

            Action leerZiehen = () => nachher.DreiZiehen();
            leerZiehen.Should().Throw<Exception>();
        }
    }
}
