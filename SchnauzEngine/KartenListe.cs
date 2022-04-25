using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchnauzEngine
{
    public class KartenListe
    {
        public IReadOnlyList<Karte> Karten { get; }

        protected KartenListe(IEnumerable<Karte> neueKarten)
        {
            Karten = neueKarten.ToList().AsReadOnly();
            if (Karten.Distinct().Count() != Karten.Count)
            {
                throw new ArgumentException("Karten sind nicht eindeutig.", nameof(neueKarten));
            }
        }

        public override bool Equals(object obj)
        {
            return obj is KartenListe liste &&
                   EqualityComparer<IReadOnlyList<Karte>>.Default.Equals(Karten, liste.Karten);
        }

        public override int GetHashCode()
        {
            return -822043384 + EqualityComparer<IReadOnlyList<Karte>>.Default.GetHashCode(Karten);
        }

        public override string ToString()
        {
            return string.Join(", ", Karten);
        }
    }
}
