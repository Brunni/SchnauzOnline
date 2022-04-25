using SchnauzEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SchnauzEngineTest
{
    public class AblaufTest
    {
        private readonly ITestOutputHelper output;

        public AblaufTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test()
        {
            var stapel = new Stapel();
            List<Hand> haende = new List<Hand>();

            for(int i=0; i<6; i++)
            {
                (var neuerStapel, var hand) = stapel.DreiZiehen();
                stapel = neuerStapel;
                haende.Add(hand);
            }

            output.WriteLine("Stapel:" + stapel);

            foreach(var hand in haende)
            {
                output.WriteLine("H: " + hand);
            }
        }
    }
}
