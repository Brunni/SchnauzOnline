using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;

namespace SchnauzEngine
{
    public class Stapel : KartenListe
    {        

        public Stapel() : base(Mischen(GeneriereKarten()))
        {
        }

        private Stapel(IEnumerable<Karte> neueKarten) : base(neueKarten)
        {
        }


        private static IEnumerable<Karte> GeneriereKarten()
        {
            return Zahlwert.Values.SelectMany(zahlwert => Farbwert.Values.Select(farbwert => new Karte(zahlwert, farbwert)));
        }

        [Pure]
        private static IEnumerable<Karte> Mischen(IEnumerable<Karte> karten)
        {
            var random = new Random();
            return karten.OrderBy(k => random.Next());
        }

        [Pure]
        public (Stapel, Hand) DreiZiehen()
        {
            if (Karten.Count < 3)
            {
                throw new Exception("Keine Karten mehr auf dem Stapel!");
            }
            var gezogen = new Hand(Karten.Take(3));
            return (new Stapel(Karten.Skip(3)), gezogen);
        }

        public override string ToString()
        {
            return $"{base.ToString()} ({Karten.Count})";
        }
    }
}
