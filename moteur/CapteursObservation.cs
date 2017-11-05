using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moteur
{
    public class CapteurObservation
    {
        public int[] ObserverCarte(Environnement env)
        {
            return env.getCarte();
        }
        public int ObserverPerformance(Environnement env)
        {
            return env.getMesurePerformance();
        }
    }
}
