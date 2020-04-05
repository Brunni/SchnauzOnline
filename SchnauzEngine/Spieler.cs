using System;
using System.Collections.Generic;
using System.Text;

namespace SchnauzEngine
{
    public class Spieler
    {
        public int Chips { get; }

        public bool NochDabei { get; }
        public string Id { get; }

        public Spieler(string id = null)
        {
            Chips = 3;
            NochDabei = true;
            Id = id ?? Guid.NewGuid().ToString();
        }

        public Spieler(Spieler spieler, int chipDelta)
        {
            Id = spieler.Id;
            if (spieler.Chips == 0)
            {
                Chips = 0;
                NochDabei = false;
            }
            else
            {
                Chips = spieler.Chips + chipDelta;
                NochDabei = true;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Spieler spieler &&
                   Id == spieler.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
