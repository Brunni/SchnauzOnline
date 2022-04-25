using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SchnauzEngine
{
    public class Hand : KartenListe
    {
        public Hand(IEnumerable<Karte> neueKarten) : base(neueKarten)
        {
            if (Karten.Count != 3)
            {
                throw new ArgumentException("Immer 3 Karten auf der Hand", nameof(neueKarten));
            }
        }

        /// <summary>
        /// Tausche eine Karte, aber halte Sortierung der anderen in beiden Händen.
        /// </summary>
        /// <returns>Tuple(SpielerHand, MitteHand)</returns>
        public (Hand, Hand) TauscheEine(Hand mitte, Karte karteMitte, Karte eigeneKarte)
        {
            var neueMitte = new Hand(mitte.Karten.Select(k => k == karteMitte ? eigeneKarte : k));
            Hand neueSpielerHand = new Hand(Karten.Select(k => k == eigeneKarte ? karteMitte : k));
            return (neueSpielerHand, neueMitte);
        }

        public bool IsFeuer => Karten.All(k => k.Zahlwert == Zahlwert.Ass);

        public bool IsSchnauz => HoechsteWertSumme == 31;

        /// <summary>
        /// Feuer nicht mit eingeschlossen.
        /// </summary>
        public bool IsDreiGleiche => Karten.Where(k => k.Zahlwert != Zahlwert.Ass).Select(k => k.Zahlwert).Distinct().Count() == 1;

        public decimal Wert => IsDreiGleiche ? 30.5M : HoechsteWertSumme;

        private int HoechsteWertSumme => Karten.GroupBy(k => k.Farbwert).Select(g => g.Sum(k => k.Zahlwert.Wert)).Max();

    }
}
