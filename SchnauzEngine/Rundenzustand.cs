using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SchnauzEngine
{
    public class Rundenzustand
    {
        public bool SpielIstZuEnde => Verlierer.Count > 0;

        public IReadOnlyList<Spieler> Verlierer { get; }

        public Rundenzustand(Runde runde)
        {
            foreach((Spieler spieler, Hand hand) in runde.SpielerHaende)
            {
                if (hand.IsFeuer)
                {
                    Verlierer = runde.SpielerListe.Where(s => s != spieler).ToList().AsReadOnly();
                }
                if (hand.IsSchnauz)
                {
                    Verlierer = BestimmeVerlierer(runde.SpielerHaende);
                }
            }
            if (runde.Dran == runde.Gekloppft)
            {
                Verlierer = BestimmeVerlierer(runde.SpielerHaende);
            }
        }

        private IReadOnlyList<Spieler> BestimmeVerlierer(IReadOnlyList<(Spieler, Hand)> spielerHaende)
        {
            decimal verlierPunkte = spielerHaende.Select(t => t.Item2.Wert).Min();
            return spielerHaende.Where(t => t.Item2.Wert == verlierPunkte).Select(t => t.Item1).ToList().AsReadOnly();
        }

    }
}