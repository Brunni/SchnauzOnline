using FluentAssertions;
using SchnauzEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SchnauzEngineTest
{
    public class TischTest
    {
        private Spieler spieler1;
        private Tisch tisch;

        public TischTest()
        {
            spieler1 = new Spieler();
            tisch = new Tisch(spieler1);
        }

        [Fact]
        public void TestTeilnehmen_Verlassen()
        {
            var spieler2 = new Spieler();
            var tisch2 = tisch.Teilnehmen(spieler2);
            tisch2.Spieler.Should().HaveCount(2);
            var tisch3 = tisch2.Verlassen(spieler2);
            tisch3.Spieler.Should().HaveCount(1);
        }

        [Fact]
        public void RundeStarten()
        {
            var spieler2 = new Spieler();
            var tisch2 = tisch.Teilnehmen(spieler2);
            tisch2.Geber.Should().Be(spieler1);
            Runde neueRunde = tisch2.RundeStarten();

            neueRunde.MitteNochNichtKlar.Should().BeTrue();
            neueRunde.SpielerListe.Should().Contain(spieler2);
            neueRunde.GetHand(spieler2).Karten.Should().HaveCount(3);

            neueRunde.Dran.Should().Be(spieler1);
            Hand spieler1Hand = neueRunde.GetHand(spieler1);

            // Geber nimmt Karten
            Runde spieler2Dran = neueRunde.GeberNimmtAktuelleKarten();
            spieler2Dran.GetHand(spieler1).Should().Be(spieler1Hand);
            spieler2Dran.Dran.Should().Be(spieler2);
            spieler2Dran.MitteNochNichtKlar.Should().BeFalse();

            var handSpieler2 = spieler2Dran.GetHand(spieler2);
            spieler2Dran.TauscheEines(handSpieler2.Karten.First(), spieler2Dran.Mitte.Karten.First());

            var rundenzustand = new Rundenzustand(spieler2Dran);
            rundenzustand.Verlierer.Should().BeNullOrEmpty();
        }

        [Fact]
        public void Einmal_Schieben()
        {
            var spieler2 = new Spieler();
            Runde vorSchieben = tisch.Teilnehmen(spieler2)
                            .RundeStarten()
                            .GeberNimmtAndereKarten();
            var runde = vorSchieben
                .Schieben();
            runde.Geschoben.Should().HaveCount(1);
            vorSchieben.Mitte.Karten.Should().BeEquivalentTo(runde.Mitte.Karten);
            vorSchieben.GetHand(spieler2).Karten.Should().BeEquivalentTo(runde.GetHand(spieler2).Karten);
        }

        [Fact]
        public void Zweimal_Schieben()
        {
            var spieler2 = new Spieler();
            Runde einmalSchieben = tisch.Teilnehmen(spieler2)
                            .RundeStarten()
                            .GeberNimmtAndereKarten()
                            .Schieben();
            var runde = einmalSchieben
                .Schieben();
            runde.Geschoben.Should().HaveCount(0);

            einmalSchieben.GetHand(spieler2).Karten.Should().BeEquivalentTo(runde.GetHand(spieler2).Karten);
            // Neue Mitte
            einmalSchieben.Mitte.Karten.Should().NotBeEquivalentTo(runde.Mitte.Karten);
        }

        [Fact]
        public void Klopfen()
        {
            var spieler2 = new Spieler();
            Tisch tisch1 = tisch.Teilnehmen(spieler2);
            tisch1.Geber.Should().Be(spieler1);
            var runde = tisch1
                .RundeStarten()
                .GeberNimmtAndereKarten()
                .Klopfen()
                .Klopfen();

            runde.Gekloppft.Should().Be(spieler2);

            var zustand = new Rundenzustand(runde);
            zustand.SpielIstZuEnde.Should().BeTrue();
            Tisch neuerTisch = tisch1.RundeBeenden(zustand);
            neuerTisch.Spieler.Where(s => s.Chips == 2).Should().HaveCountGreaterOrEqualTo(1);
            neuerTisch.Geber.Id.Should().Be(spieler2.Id);
        }

    }
}
