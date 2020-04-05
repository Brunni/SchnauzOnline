using FluentAssertions;
using SchnauzEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SchnauzEngineTest
{
    public class HandTest
    {
        private readonly Hand hand;
        private readonly Hand mitte;
        public HandTest()
        {
            Stapel stapel = new Stapel();
            (stapel, hand) = stapel.DreiZiehen();
            (_, mitte) = stapel.DreiZiehen();
        }

        [Fact]
        public void TauscheEine_Alle_Karten_noch_da()
        {
            (var neueHand, var neueMitte) = hand.TauscheEine(mitte, mitte.Karten.First(), hand.Karten.First());

            var neueKarten = neueHand.Karten.ToList();
            neueKarten.AddRange(neueMitte.Karten);

            var alteKarten = hand.Karten.ToList();
            alteKarten.AddRange(mitte.Karten);

            neueKarten.Should().BeEquivalentTo(alteKarten);
        }

        [Fact]
        public void TauscheEine_Karte_getauscht()
        {
            Karte ausMitte = mitte.Karten.First();
            Karte ausHand = hand.Karten.First();
            (var neueHand, var neueMitte) = hand.TauscheEine(mitte, ausMitte, ausHand);

            neueMitte.Karten.Should().Contain(ausHand);
            neueHand.Karten.Should().Contain(ausMitte);
        }

        [Fact]
        public void TauscheEine_ErsteKarten_Reihenfolge_gleich()
        {
            Karte ausMitte = mitte.Karten.First();
            Karte ausHand = hand.Karten.First();
            (var neueHand, var neueMitte) = hand.TauscheEine(mitte, ausMitte, ausHand);

            neueHand.Karten.Skip(1).Should().ContainInOrder(hand.Karten.Skip(1));
            neueMitte.Karten.Skip(1).Should().ContainInOrder(mitte.Karten.Skip(1));
        }

        [Fact]
        public void TauscheEine_ZweiteGegenDritteKarten_Reihenfolge_gleich()
        {
            Karte ausMitte = mitte.Karten.Skip(1).First();
            Karte ausHand = hand.Karten.Skip(2).First();
            (var neueHand, var neueMitte) = hand.TauscheEine(mitte, ausMitte, ausHand);

            neueHand.Karten.Take(2).Skip(1).Should().ContainInOrder(hand.Karten.Take(2).Skip(1));
            neueMitte.Karten.Take(1).Skip(1).Take(1).Should().ContainInOrder(mitte.Karten.Take(1).Skip(1).Take(1));
        }

        [Fact]
        public void SchnauzHand_IstSchnauz_Hat31Wert()
        {
            var karten = new[] { new Karte(Zahlwert.Zehn, Farbwert.Eichel), new Karte(Zahlwert.Ass, Farbwert.Eichel), new Karte(Zahlwert.Ober, Farbwert.Eichel) };

            Hand schnauzHand = new Hand(karten);
            schnauzHand.Wert.Should().Be(31);
            schnauzHand.IsSchnauz.Should().BeTrue();
            schnauzHand.IsFeuer.Should().BeFalse();
        }

        [Fact]
        public void KeinSchnauz_FalscheFarbe_IsFalse()
        {
            var karten = new[] { new Karte(Zahlwert.Zehn, Farbwert.Eichel), new Karte(Zahlwert.Ass, Farbwert.Gras), new Karte(Zahlwert.Ober, Farbwert.Eichel) };

            Hand zweiPassendeKartenHand = new Hand(karten);
            zweiPassendeKartenHand.Wert.Should().Be(20);
            zweiPassendeKartenHand.IsSchnauz.Should().BeFalse();
            zweiPassendeKartenHand.IsFeuer.Should().BeFalse();
        }

        [Fact]
        public void SchlechteHand_WenigPunkte()
        {
            var karten = new[] { new Karte(Zahlwert.Sechs, Farbwert.Eichel), new Karte(Zahlwert.Sieben, Farbwert.Gras), new Karte(Zahlwert.Acht, Farbwert.Herz) };

            Hand schlechteHand = new Hand(karten);
            schlechteHand.Wert.Should().Be(8);
            schlechteHand.IsFeuer.Should().BeFalse();
            schlechteHand.IsSchnauz.Should().BeFalse();
        }

        [Fact]
        public void DreiAsse_Feuer()
        {
            var karten = new[] { new Karte(Zahlwert.Ass, Farbwert.Eichel), new Karte(Zahlwert.Ass, Farbwert.Gras), new Karte(Zahlwert.Ass, Farbwert.Herz) };

            Hand feuerHand = new Hand(karten);
            feuerHand.Wert.Should().Be(11); // Feuer: Wert egal
            feuerHand.IsFeuer.Should().BeTrue();
            feuerHand.IsSchnauz.Should().BeFalse();
            feuerHand.IsDreiGleiche.Should().BeFalse();
        }

        [Fact]
        public void DreiGleiche_Feuer()
        {
            var karten = new[] { new Karte(Zahlwert.Sechs, Farbwert.Eichel), new Karte(Zahlwert.Sechs, Farbwert.Gras), new Karte(Zahlwert.Sechs, Farbwert.Herz) };

            Hand dreiGleiche = new Hand(karten);
            dreiGleiche.Wert.Should().Be(30.5M);
            dreiGleiche.IsFeuer.Should().BeFalse();
            dreiGleiche.IsSchnauz.Should().BeFalse();
            dreiGleiche.IsDreiGleiche.Should().BeTrue();
        }
    }
}
