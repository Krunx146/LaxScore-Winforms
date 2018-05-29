using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lax_score
{
    public class Iskljucenja
    {
        public int Broj;
        public string Prezime;
        public int Trajanje;

        public Iskljucenja(int br, string pr, int tr)
        {
            Broj = br;
            Prezime = pr;
            Trajanje = tr;
        }
        public override string ToString()
        {
            string tempStr = Broj + " " + Prezime.ToUpper() + " " + Trajanje;
            return tempStr;
        }
    }
}
