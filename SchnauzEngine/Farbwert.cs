using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SchnauzEngine
{
    public class Farbwert
    {
        public string AnzeigeName { get; }

        private Farbwert(string anzeigeName)
        {
            AnzeigeName = anzeigeName;
        }

        public override string ToString()
        {
            return AnzeigeName;
        }

        public static Farbwert Eichel = new Farbwert("Eichel");

        public static Farbwert Gras = new Farbwert("Gras");

        public static Farbwert Herz = new Farbwert("Herz");

        public static Farbwert Schellen = new Farbwert("Schellen");

        public static IEnumerable<Farbwert> Values { get; } = typeof(Farbwert).GetFields(BindingFlags.Static | BindingFlags.Public).Select(f => (Farbwert)f.GetValue(null)).ToList();

    }
}
