using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchnauzEngine
{
    public class Tisch
    {
        /// <summary>
        /// Spieler im Uhrzeigersinn (0 ist bei 12 Uhr)
        /// </summary>
        public IReadOnlyList<Spieler> Spieler { get; }

        public Spieler Geber { get; }

        public Tisch(Spieler spieler, string id = null) : this(new[] { spieler }, null, id)
        {
        }

        private Tisch(IEnumerable<Spieler> spieler, Spieler geber, string id = null)
        {
            this.Spieler = spieler.ToList().AsReadOnly();
            this.Geber = geber ?? this.Spieler.First();
            Id = id ?? Guid.NewGuid().ToString();
        }

        public Tisch Teilnehmen(Spieler neuerSpieler)
        {
            var spielerListe = Spieler.ToList();
            spielerListe.Add(neuerSpieler);
            var geber = Geber ?? neuerSpieler;
            return new Tisch(spielerListe, geber, Id);
        }

        public Tisch Verlassen(Spieler verlassenderSpieler)
        {
            var spielerListe = Spieler.ToList();

            Spieler neuerGeber = Geber;
            if (verlassenderSpieler == Geber)
            {
                NeuenGeberBestimmen(spielerListe);
            }
            spielerListe.Remove(verlassenderSpieler);

            return new Tisch(spielerListe, neuerGeber, Id);
        }

        public Runde RundeStarten()
        {
            var runde = new Runde(TeilnehmendeSpieler.ToList(), Geber);
            return runde;
        }

        public Tisch RundeBeenden(Rundenzustand rundenzustand)
        {
            IReadOnlyList<Spieler> verlierer = rundenzustand.Verlierer;
            var neueSpieler = Spieler.Select(s => verlierer.Contains(s) ? new Spieler(s, -1) : s).ToList();
            return new Tisch(neueSpieler, NeuenGeberBestimmen(neueSpieler));
        }

        private IEnumerable<Spieler> TeilnehmendeSpieler => Spieler.Where(s => s.NochDabei);

        public string Id { get; }

        private Spieler NeuenGeberBestimmen(List<Spieler> spielerListe)
        {
            Spieler kandidat = Geber;
            do
            {
                int index = spielerListe.FindIndex((s) => s == kandidat);
                kandidat = spielerListe.ToList().ElementAtOrDefault(index + 1) ?? spielerListe.First();
            }
            while (!spielerListe.Where(s => s.NochDabei).Contains(kandidat));

            return kandidat;
        }
    }
}
