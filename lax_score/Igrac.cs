using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lax_score
{
    public class Igrac
    {
        public int Broj;
        public string Ime;
        public string Prezime;
        public bool Goalie;

        public int saves;
        public int shots;
        public int shotsOnGoal;
        public int goals;
        public int assists;
        public int groundBalls;
        public int error;
        public int interception;
        public int turnover;
        public int penalty;
        public int faceoff;
        public int faceoffWin;

        public Igrac(int br, string im, string prim)
        {
            Broj = br;
            Ime = im;
            Prezime = prim;
            Goalie = false;
            saves = 0;
            shots = 0;
            shotsOnGoal = 0;
            goals = 0;
            assists = 0;
            groundBalls = 0;
            error = 0;
            interception = 0;
            turnover = 0;
            penalty = 0;
            faceoff = 0;
            faceoffWin = 0;
        }

        public Igrac(string br, string im, string prim, string sav, string sho, string sog, string goa, string ass, string gb, string e, string inter, string tur, string pen, string foff, string foffw)
        {
            Broj = int.Parse(br);
            Ime = im;
            Prezime = prim;
            saves = int.Parse(sav);
            if (saves > 0) Goalie = true;
            shots = int.Parse(sho);
            shotsOnGoal = int.Parse(sog);
            goals = int.Parse(goa);
            assists = int.Parse(ass);
            groundBalls = int.Parse(gb);
            error = int.Parse(e);
            interception = int.Parse(inter);
            turnover = int.Parse(tur);
            penalty = int.Parse(pen);
            faceoff = int.Parse(foff);
            faceoffWin = int.Parse(foffw);
        }

        public void JoinPlayer(Igrac postojeci)
        {
            this.saves += postojeci.saves;
            this.shots += postojeci.shots;
            this.shotsOnGoal += postojeci.shotsOnGoal;
            this.goals += postojeci.goals;
            this.assists += postojeci.assists;
            this.groundBalls += postojeci.groundBalls;
            this.error += postojeci.error;
            this.interception += postojeci.interception;
            this.turnover += postojeci.turnover;
            this.penalty += postojeci.penalty;
            this.faceoff += postojeci.faceoff;
            this.faceoffWin += postojeci.faceoffWin;
        }

        public void AddSave()
        {
            saves++;
        }
        public void AddShot(bool onGoal)
        {
            shots++;
            if (onGoal) shotsOnGoal++;
        }
        public void AddGoal()
        {
            goals++;
        }
        public void AddAssist()
        {
            assists++;
        }
        public void AddGroundball()
        {
            groundBalls++;
        }
        public void AddE()
        {
            error++;
        }
        public void AddIN()
        {
            interception++;
        }
        public void AddT()
        {
            turnover++;
        }
        public void AddPenalty(int value)
        {
            penalty += value;
        }
        public void AddFaceoff(bool win)
        {
            faceoff++;
            if (win) faceoffWin++;
        }
        public string GetPlayerString()
        {
            string plString = Broj + "," + Ime + "," + Prezime + "," + saves + "," + shotsOnGoal + "," + shots +
                "," + goals + "," + assists + "," + groundBalls + "," + error + "," + interception +
                "," + turnover + "," + penalty + "," + faceoffWin + "," + faceoff + ",";
            return plString;
        }
        public override string ToString()
        {
            if(Goalie == true)
            {
                return Broj + "   " + Ime.ToUpper() + " " + Prezime.ToUpper() + " (GK)";
            }
            return Broj + "   " + Ime.ToUpper() + " " + Prezime.ToUpper();
        }
    }
}
