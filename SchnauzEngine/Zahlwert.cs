using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SchnauzEngine
{
    public class Zahlwert
    {
        public string AnzeigeName { get; }

        public int Wert { get;  }

        private Zahlwert(string anzeigeName, int wert)
        {
            AnzeigeName = anzeigeName;
            Wert = wert;
        }

        private Zahlwert(int zahlWert)
        {
            AnzeigeName = zahlWert.ToString();
            Wert = zahlWert;
        }


        public override string ToString()
        {
            return AnzeigeName;
        }

        public static Zahlwert Ass = new Zahlwert("Ass", 11);

        public static Zahlwert Ober = new Zahlwert("Ober", 10);

        public static Zahlwert Unter = new Zahlwert("Unter", 10);

        public static Zahlwert Zehn = new Zahlwert(10);

        public static Zahlwert Neun = new Zahlwert(9);

        public static Zahlwert Acht = new Zahlwert(8);

        public static Zahlwert Sieben = new Zahlwert(7);

        public static Zahlwert Sechs = new Zahlwert(6);

        public static IEnumerable<Zahlwert> Values { get; } = typeof(Zahlwert).GetFields(BindingFlags.Static | BindingFlags.Public).Select(f => (Zahlwert)f.GetValue(null)).ToList();
    }
}
