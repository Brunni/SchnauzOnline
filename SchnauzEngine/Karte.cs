using System;
using System.Collections.Generic;
using System.Text;

namespace SchnauzEngine
{
    public class Karte
    {
        public Zahlwert Zahlwert { get; }

        public Farbwert Farbwert { get; }

        public Karte(Zahlwert zahlwert, Farbwert farbwert)
        {
            Zahlwert = zahlwert;
            Farbwert = farbwert;
        }

        public override string ToString()
        {
            return $"{Farbwert} {Zahlwert}";
        }

        public override bool Equals(object obj)
        {
            return obj is Karte karte &&
                   EqualityComparer<Zahlwert>.Default.Equals(Zahlwert, karte.Zahlwert) &&
                   EqualityComparer<Farbwert>.Default.Equals(Farbwert, karte.Farbwert);
        }

        public override int GetHashCode()
        {
            int hashCode = 1529419808;
            hashCode = hashCode * -1521134295 + EqualityComparer<Zahlwert>.Default.GetHashCode(Zahlwert);
            hashCode = hashCode * -1521134295 + EqualityComparer<Farbwert>.Default.GetHashCode(Farbwert);
            return hashCode;
        }
    }
}
