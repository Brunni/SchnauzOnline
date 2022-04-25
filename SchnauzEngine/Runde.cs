using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchnauzEngine
{
    public class Runde
    {
        private Stapel stapel;

        public Hand Mitte { get; }

        public bool MitteNochNichtKlar { get; }
        public Spieler Gekloppft { get; }

        public IReadOnlyList<Spieler> Geschoben { get; }
        public IReadOnlyList<(Spieler, Hand)> SpielerHaende { get; }

        public Spieler Dran { get; }

        public IEnumerable<Spieler> SpielerListe => SpielerHaende.Select(t => t.Item1);

        public Hand GetHand(Spieler spieler) => SpielerHaende.Single(t => t.Item1 == spieler).Item2;

        /// <summary>
        /// Neue Runde starten
        /// </summary>
        public Runde(IReadOnlyList<Spieler> spieler, Spieler geber)
        {
            if (spieler.Count < 2)
            {
                throw new Exception("Nicht genug Spieler");
            }
            (SpielerHaende, Mitte) = Austeilen(spieler);
            Dran = geber;
            MitteNochNichtKlar = true;
            Geschoben = new List<Spieler>();
        }

        private Runde(Stapel stapel, IEnumerable<(Spieler, Hand)> neueHandListe, Hand neueMitte, Spieler naechsterSpieler, Spieler gekloppft, IReadOnlyList<Spieler> geschoben = null)
        {
            SpielerHaende = neueHandListe.ToList().AsReadOnly();
            Dran = naechsterSpieler;
            Mitte = neueMitte;
            MitteNochNichtKlar = false;
            Gekloppft = gekloppft;
            Geschoben = geschoben ?? new List<Spieler>().AsReadOnly();
            this.stapel = stapel;
        }

        private (List<(Spieler, Hand)> neueListe, Hand mitte) Austeilen(IReadOnlyList<Spieler> spieler)
        {
            stapel = new Stapel();

            var neueListe = new List<(Spieler, Hand)>();
            foreach (var s in spieler)
            {
                Hand hand;
                (stapel, hand) = stapel.DreiZiehen();
                neueListe.Add((s, hand));
            }
            (_, var mitte) = stapel.DreiZiehen();
            return (neueListe, mitte);
        }

        public Runde GeberNimmtAktuelleKarten()
        {
            if (MitteNochNichtKlar == false)
            {
                throw new Exception("Falscher Zustand");
            }

            return new Runde(stapel, SpielerHaende, Mitte, BestimmeNaechstenSpieler(), Gekloppft, Geschoben);
        }

        public Runde GeberNimmtAndereKarten()
        {
            if (MitteNochNichtKlar == false)
            {
                throw new Exception("Falscher Zustand");
            }
            return TauscheAlleDrei();
        }

        public Runde TauscheEines(Karte spielerKarte, Karte mitteKarte)
        {
            if (MitteNochNichtKlar == true)
            {
                throw new Exception("Falscher Zustand");
            }
            Hand aktuelleHand = GetAktuelleHand();
            (var neueSpielerHand, var neueMitte) = aktuelleHand.TauscheEine(Mitte, mitteKarte, spielerKarte);

            var neueHandListe = ErsetzeHand(neueSpielerHand);
            Spieler naechsterSpieler = BestimmeNaechstenSpieler();
            return new Runde(stapel, neueHandListe, neueMitte, naechsterSpieler, Gekloppft);
        }

        public Runde TauscheAlleDrei()
        {
            Hand neueMitte = GetAktuelleHand();
            var neueHandListe = ErsetzeHand(Mitte);
            Spieler naechsterSpieler = BestimmeNaechstenSpieler();
            return new Runde(stapel, neueHandListe, neueMitte, naechsterSpieler, Gekloppft);
        }

        public Runde Schieben()
        {
            if (MitteNochNichtKlar == true)
            {
                throw new Exception("Falscher Zustand");
            }
            var geschoben = Geschoben.ToList();
            geschoben.Add(Dran);
            Hand neueMitte;
            if (geschoben.Count == SpielerHaende.Count)
            {
                (stapel, neueMitte) = stapel.DreiZiehen();
                geschoben = null;
            } else
            {
                neueMitte = Mitte;
            }
            return new Runde(stapel, this.SpielerHaende, neueMitte, BestimmeNaechstenSpieler(), Gekloppft, geschoben);
        }

        public Runde Klopfen()
        {
            if (MitteNochNichtKlar == true)
            {
                throw new Exception("Falscher Zustand");
            }
            var gekloppft = Gekloppft ?? Dran;
            return new Runde(stapel, this.SpielerHaende, this.Mitte, BestimmeNaechstenSpieler(), gekloppft);
        }

        private Spieler BestimmeNaechstenSpieler()
        {
            var spielerListe = SpielerListe.ToList();
            int index = spielerListe.FindIndex((s) => s == Dran);
            return spielerListe.ToList().ElementAtOrDefault(index + 1) ?? spielerListe.First();
        }

        private Hand GetAktuelleHand()
        {
            return GetHand(Dran);
        }

        private List<(Spieler, Hand)> ErsetzeHand(Hand neueHand)
        {
            return SpielerHaende.Select((t) => t.Item1 == Dran ? (t.Item1, neueHand) : t).ToList();
        }
    }
}
